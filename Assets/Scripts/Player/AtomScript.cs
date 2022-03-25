using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class AtomScript : MonoBehaviour
{
    Vector2 movementDirection = Vector2.zero;
    [SerializeField] float movementForce = 500;

    [SerializeField] uint protons = 0;
    [SerializeField] uint neutrons = 0;
    [SerializeField] uint electrons = 0;

    Rigidbody2D rb;


    public void addProton(uint n = 1)
    {
        protons += n;
        PrintStatus();
    }

    public void addNeutron(uint n = 1)
    {
        neutrons += n;
        PrintStatus();
    }

    public void addElectron(uint n = 1)
    {
        electrons += n;
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
}
