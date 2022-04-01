using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using hjijijing.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class AtomScript : MonoBehaviour
{
    Vector2 movementDirection = Vector2.zero;
    [SerializeField] float movementForce = 500;

    SpriteRenderer sr;

    [SerializeField] uint protons = 0;
    [SerializeField] uint neutrons = 0;
    [SerializeField] uint electrons = 0;

    public delegate void ProtonsAdded(uint before, uint after, uint change, AtomScript atomScript);
    public ProtonsAdded onProtonsAdded;
    public delegate void NeutronsAdded(uint before, uint after, uint change, AtomScript atomScript);
    public NeutronsAdded onNeutronsAdded;
    public delegate void ElectronsAdded(uint before, uint after, uint change, AtomScript atomScript);
    public ElectronsAdded onElectronsAdded;

    Rigidbody2D rb;


    [SerializeField] Vector2 halfLifeRange = Vector2.zero;
    TweeningAnimation isotopeAnimation;

    [SerializeField] float electronProtonBalanceDelay = 5f;
    [SerializeField] int maxElectronProtonDifference = 1;
    TweeningAnimation electronProtonAnimation;

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

    public void addProton(uint n = 1, ParticleScript particle = null)
    {
        protons += n;
        onProtonsAdded?.Invoke(protons - n, protons, n, this);
        CheckElectronProtonBalance();
        CheckIsotope();
        PrintStatus();
        CoreParticlePickedUp(particle);
        UpdateMass();
    }

    public void addNeutron(uint n = 1, ParticleScript particle = null)
    {
        neutrons += n;
        onNeutronsAdded?.Invoke(neutrons - n, neutrons, n, this);
        CheckIsotope();
        PrintStatus();
        CoreParticlePickedUp(particle);
        UpdateMass();
    }

    public void addElectron(uint n = 1, ParticleScript particle = null)
    {
        electrons += n;
        onElectronsAdded?.Invoke(electrons - n, electrons, n, this);
        CheckElectronProtonBalance();
        PrintStatus();
        ShellParticlePickedUp(particle);
        UpdateMass();
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

        StartShellParticleAnimation();
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
        Vector2 force = movementDirection * Time.fixedDeltaTime * movementForce;

        rb.MovePosition(transform.position + new Vector3(force.x, force.y, 0));
        //rb.AddForce(force);

    }


    public void OnTranslation(InputValue value)
    {
        movementDirection = value.Get<Vector2>().normalized;
    }




    void CheckElectronProtonBalance()
    {
        int difference = (int)electrons - (int)protons;

        if(Mathf.Abs(difference) >= maxElectronProtonDifference)
        {
            if (electronProtonAnimation != null) return;
            electronProtonAnimation = new TweeningAnimation(this, gameObject);
            Countdown countdown = Instantiate(countdownPrefab, canvas.transform).GetComponent<Countdown>();
            countdown.y = -100;

            if (electrons > protons)
                countdown.color = Color.red;
            else
                countdown.color = Color.blue;

            float animateInDuration = 0.2f;
            float animateOutDuration = electronProtonBalanceDelay - animateInDuration;

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

        } else
        {
            electronProtonAnimation.revert();
            electronProtonAnimation = null;
        }

    }

    void CheckIsotope()
    {
        Isotope newIsotope = IsotopeManager.isotopeManager.GetIsotope((int)protons, (int)neutrons);

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

            float animateInDuration = 0.2f;
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
        StartCoreAnimation(ps.gameObject);
    }

    void StartCoreAnimation(GameObject coreParticle)
    {
        Vector3 currentOffset = coreParticle.transform.position - transform.position;
        float angle = Random.Range(0, Mathf.PI * 2);
        float r = Random.value;

        Vector3 newOffset = new Vector3(Mathf.Cos(angle) * r,Mathf.Sin(angle)*r,0);
        newOffset *= coreRadiusStart + ((float)(protons+neutrons))*coreRadiusIncrease;

        new TweeningAnimation(this, coreParticle)
            .vector3Callback(currentOffset, newOffset, (p) => { coreParticle.transform.position = transform.position + p; }, 0.3f)
            .SetEasing(Easing.easeInOutSine)
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

    public float shellRotationSpeed = 2 * Mathf.PI;


    void ShellParticlePickedUp(ParticleScript ps)
    {
        if (ps == null) return;

        int shellNumber = AtomUtil.getOuterShell((int)electrons);
        shellAmounts[shellNumber - 1]++;

        int numberInShell = shellAmounts[shellNumber - 1];

        shells[shellNumber - 1].Add(ps);
    }

    
    void StartShellParticleAnimation()
    {
        new TweeningAnimation(this)
            .floatCallback(0f, Mathf.PI*2, (c) =>
            {
                for (int i = 0; i < 7; i++)
                {
                    int shellNumber = i + 1;
                    for (int j = 0; j < shells[shellNumber - 1].Count; j++)
                    {
                        float angleOffset =(Mathf.PI/7 * i) + j * (Mathf.PI * 2 / (float)shellAmounts[shellNumber - 1]);
                        float theta = angleOffset + c;


                        float r = innerShellRadius + distanceBetweenShells * i;

                        Vector3 pos = transform.position + new Vector3(Mathf.Cos(theta) * r, Mathf.Sin(theta) * r, 0f);

                        shells[i][j].transform.position = pos;
                    }
                }
            }, shellRotationSpeed)
            .then()
            .call(StartShellParticleAnimation)
            .Start();
    }


    void UnstableForTooLong()
    {
        Debug.Log("Unstable isotope!");
        Time.timeScale = 0;
    }

}
