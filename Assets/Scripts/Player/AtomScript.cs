using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hjijijing.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class AtomScript : MonoBehaviour
{
    Vector2 movementDirection = Vector2.zero;
    [SerializeField] float startSpeed = 6;

    SpriteRenderer sr;

    [SerializeField] uint protons = 0;
    [SerializeField] uint neutrons = 0;
    [SerializeField] uint electrons = 0;

    public Isotope isotope { get; private set; }

    public delegate void ProtonsAdded(uint before, uint after, uint change, AtomScript atomScript);
    public ProtonsAdded onProtonsAdded;
    public delegate void NeutronsAdded(uint before, uint after, uint change, AtomScript atomScript);
    public NeutronsAdded onNeutronsAdded;
    public delegate void ElectronsAdded(uint before, uint after, uint change, AtomScript atomScript);
    public ElectronsAdded onElectronsAdded;

    Rigidbody2D rb;


    [SerializeField] Vector2 halfLifeRange = Vector2.zero;
    public TweeningAnimation isotopeAnimation;

    [SerializeField] float electronProtonBalanceDelay = 5f;
    [SerializeField] float electronProtonDelayFalloff = 1f;
    [SerializeField] float electronProtonMinimumDelay = 1f;
    [SerializeField] int maxElectronProtonDifference = 1;
    public TweeningAnimation electronProtonAnimation;

    [SerializeField] GameObject countdownPrefab;
    [SerializeField] GameObject canvas;


    public uint getProtons()
    {
        return protons;
    }
    public uint getNeutrons()
    {
        return neutrons;
    }
    public uint getElectrons()
    {
        return electrons;
    }


    public float getCharge()
    {
        return (float)getProtons() - (float)getElectrons();
    }

    public bool addProton(uint n = 1, ParticleScript particle = null)
    {
        if (protons + n > AtomUtil.HIGHESTATOM || protons + n > GameManager.gameManager.targetElement.atomicNumber) return false;
        protons += n;
        CheckElectronProtonBalance();
        CheckIsotope();
        //PrintStatus();
        CoreParticlePickedUp(particle);
        UpdateMass();
        onProtonsAdded?.Invoke(protons - n, protons, n, this);
        return true;
    }

    public bool addNeutron(uint n = 1, ParticleScript particle = null)
    {
        if (neutrons + n > GameManager.gameManager.targetElement.numberOfNeutrons) return false;
        neutrons += n;
        CheckIsotope();
        //PrintStatus();
        CoreParticlePickedUp(particle);
        UpdateMass();
        onNeutronsAdded?.Invoke(neutrons - n, neutrons, n, this);
        return true;
    }

    public bool addElectron(uint n = 1, ParticleScript particle = null)
    {
        if (electrons + n > AtomUtil.HIGHESTATOM || electrons + n > GameManager.gameManager.targetElement.atomicNumber) return false;
        electrons += n;
        CheckElectronProtonBalance();
        //PrintStatus();
        ShellParticlePickedUp(particle);
        UpdateMass();
        onElectronsAdded?.Invoke(electrons - n, electrons, n, this);
        return true;
    }


    void UpdateMass()
    {
        rb.mass = GetMass();
    }

    public float GetMass()
    {
        return AtomUtil.getMass((int)protons, (int)neutrons, (int)electrons);
    }


    void PrintStatus()
    {
        Debug.Log("Protons: " + protons + " Neutrons: " + neutrons + " Electrons: " + electrons);
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        for(int i = 0; i < 7; i++)
        {
        StartShellParticleAnimation(i+1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!GameManager.gameManager.isRunning) return;

        Vector2 speed = movementDirection * Time.fixedDeltaTime * startSpeed;
        
        
        rb.MovePosition(transform.position + new Vector3(speed.x, speed.y, 0));
        //rb.AddForce(force);

    }


    public void OnTranslation(InputValue value)
    {
        movementDirection = value.Get<Vector2>().normalized;
    }




    void CheckElectronProtonBalance()
    {
        int difference = (int)electrons - (int)protons;

        electronProtonAnimation?.revert();

        if (Mathf.Abs(difference) >= maxElectronProtonDifference)
        {
            

            electronProtonAnimation = new TweeningAnimation(this, gameObject);
            Countdown countdown = Instantiate(countdownPrefab, canvas.transform).GetComponent<Countdown>();
            countdown.y = -100;

            if (electrons > protons)
                countdown.color = Color.red;
            else
                countdown.color = Color.blue;

            float animateInDuration = 0.3f;

            float delay = Mathf.Max(electronProtonMinimumDelay, electronProtonBalanceDelay - ((float)(Mathf.Abs(difference) - 1)) * electronProtonDelayFalloff);

            float animateOutDuration = delay - animateInDuration;

            countdown.StartAnimationDuration = animateInDuration;


            electronProtonAnimation
                .floatCallback(1f, 0, (f) => { countdown.SetFill(f); }, animateOutDuration, animateInDuration)
                .then()
                //.colorCallback(Color.green, new Color(0f, 0f, 0f, 0f), (c) => { sr.material.color = c; }, electronProtonBalanceDelay)
                //.then(electronProtonBalanceDelay)
                .call(UnstableForTooLong)
                .Start();

            void destroyCountdown() { countdown.Remove(); electronProtonAnimation.onRevert -= destroyCountdown; };

            electronProtonAnimation.onRevert += destroyCountdown;

        }

    }

    void CheckIsotope()
    {
        Isotope newIsotope = IsotopeManager.isotopeManager.GetIsotope((int)protons, (int)neutrons);
        isotope = newIsotope;

        isotopeAnimation?.revert();

        
        
        if(newIsotope == null || newIsotope.half_life != 0f)
        { 
            float halfLife = IsotopeManager.isotopeManager.lowestHalflife;
            if (newIsotope != null) halfLife = newIsotope.half_life;

            float duration = IsotopeManager.isotopeManager.MapHalfLife(halfLife, halfLifeRange.x, halfLifeRange.y);


            isotopeAnimation = new TweeningAnimation(this, gameObject);
            Countdown countdown = Instantiate(countdownPrefab, canvas.transform).GetComponent<Countdown>();
            countdown.y = -200;

            IsotopeManager.PICKUP_FOR_STABLE whatToPickUp = IsotopeManager.isotopeManager.findPickupForCore(protons, neutrons);
            Color c = new Color(255f, 140f, 0f);
            if (whatToPickUp == IsotopeManager.PICKUP_FOR_STABLE.PROTON)
                c = Color.red;
            else if (whatToPickUp == IsotopeManager.PICKUP_FOR_STABLE.NEUTRON)
                c = Color.yellow;

            countdown.color = c;

            float animateInDuration = 0.3f;
            float animateOutDuration = duration - animateInDuration;

            countdown.StartAnimationDuration = animateInDuration;

            isotopeAnimation
                .floatCallback(1f, 0, (f) => { countdown.SetFill(f); }, animateOutDuration, animateInDuration)
                .then()
                //.Wait(duration)
                //.scale(Vector3.zero, duration)
                //.from(Vector3.one)
                //.then(duration)
                .call(UnstableForTooLong);

            void destroyCountdown() { countdown.Remove(); isotopeAnimation.onRevert -= destroyCountdown; };

            isotopeAnimation.onRevert += destroyCountdown;

            isotopeAnimation.Start();
        }


    }


    void CoreParticlePickedUp(ParticleScript ps)
    {
        if (ps == null) return;
        if (protons + neutrons < maxCoreAnimations)
            StartCoreAnimation(ps.gameObject);
        else
            Destroy(ps.gameObject);
    }

    void StartCoreAnimation(GameObject coreParticle)
    {
        Vector3 currentOffset = coreParticle.transform.position - transform.position;
        float angle = Random.Range(0, Mathf.PI * 2);
        float r = Random.value;

        Vector3 newOffset = new Vector3(Mathf.Cos(angle) * r,Mathf.Sin(angle)*r,0);
        newOffset *= coreRadiusStart + (Mathf.Clamp((float)(protons+neutrons),0,maxCoreAnimations))*coreRadiusIncrease;

        new TweeningAnimation(this, coreParticle)
            .vector3Callback(currentOffset, newOffset, (p) => { coreParticle.transform.position = transform.position + p; }, coreAnimationDuration)
            .SetEasing(Easing.easeOutSine)
            .then()
            .call(() => { StartCoreAnimation(coreParticle); })
            .Start();
    }


    int[] shellAmounts = new int[] {0,0,0,0,0,0,0 };
    List<List<ParticleScript>> shells = new List<List<ParticleScript>>() { 
        new List<ParticleScript>(),
        new List<ParticleScript>(),
        new List<ParticleScript>(),
        new List<ParticleScript>(),
        new List<ParticleScript>(),
        new List<ParticleScript>(),
        new List<ParticleScript>()
    };


    public float innerShellRadius = 1f;
    public float distanceBetweenShells = 0.1f;

    public float coreRadiusStart = 0.2f;
    public float coreRadiusIncrease = 0.01f;

    //public float[] shellRotationSpeeds = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f };
    public float shellRotationSpeed = 2f;


    public int maxCoreAnimations = 30;
    public float coreAnimationDuration = 0.3f;

    void ShellParticlePickedUp(ParticleScript ps)
    {
        if (ps == null) return;

        int shellNumber = AtomUtil.getOuterShell((int)electrons);
        shellAmounts[shellNumber - 1]++;

        int numberInShell = shellAmounts[shellNumber - 1];

        shells[shellNumber - 1].Add(ps);
    }

    
    void StartShellParticleAnimation(int shellNumber)
    {
        int i = shellNumber - 1;

        float r = innerShellRadius + distanceBetweenShells * i;

        TweeningAnimation.Oscillate(this, (Mathf.PI * 2) / (shellRotationSpeed / r), (c) =>
        {
            for (int j = 0; j < shells[i].Count; j++)
            {
                float angleOffset = (Mathf.PI / 7 * i) + j * (Mathf.PI * 2 / (float)shellAmounts[i]);
                float theta = angleOffset + c;



                Vector3 pos = transform.position + new Vector3(Mathf.Cos(theta) * r, Mathf.Sin(theta) * r, 0f);

                shells[i][j].transform.position = pos;
            }

        }).Loop();
    }


    void UnstableForTooLong()
    {
        Debug.Log("Unstable isotope!");
        GameManager.gameManager.EndGame(EndCondition.LOSS);
    }

}
