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

    public void addProton(uint n = 1)
    {
        protons += n;
        onProtonsAdded?.Invoke(protons - n, protons, n, this);
        CheckIsotope();
        PrintStatus();
    }

    public void addNeutron(uint n = 1)
    {
        neutrons += n;
        onNeutronsAdded?.Invoke(neutrons - n, neutrons, n, this);
        CheckIsotope();
        PrintStatus();
    }

    public void addElectron(uint n = 1)
    {
        electrons += n;
        onElectronsAdded?.Invoke(electrons - n, electrons, n, this);
        PrintStatus();
    }

    void PrintStatus()
    {
        Debug.Log("Protons: " + protons + " Neutrons: " + neutrons + " Electrons: " + electrons);
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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


    void CheckIsotope()
    {
        Isotope newIsotope = IsotopeManager.isotopeManager.GetIsotope((int)protons, (int)neutrons);

        isotopeAnimation?.revert();

        if (newIsotope == null) { Debug.Log("Unknown Isotope");  return; }
        
        if(newIsotope.half_life != 0f)
        {
            isotopeAnimation = new TweeningAnimation(this, gameObject);
            float duration = IsotopeManager.isotopeManager.MapHalfLife(newIsotope.half_life, halfLifeRange.x, halfLifeRange.y);

            isotopeAnimation
                //.Wait(duration)
                .scale(Vector3.zero, duration)
                .from(Vector3.one)
                .then()
                .call(UnstableForTooLong);

            isotopeAnimation.Start();
        }


    }

    void UnstableForTooLong()
    {
        Debug.Log("Unstable isotope!");
    }

}
