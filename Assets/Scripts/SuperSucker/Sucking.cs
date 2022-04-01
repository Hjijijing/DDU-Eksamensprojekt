using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucking : MonoBehaviour
{
    public AtomScript player;
    Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<AtomScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector3 delta = player.transform.position - transform.position;
        Vector3 force = ((ScientificConstants.Constants.G * player.GetMass() * rb.mass) / (delta.sqrMagnitude*ScientificConstants.Constants.DistanceScale)) * delta.normalized;

        rb.AddForce(force);
    }
}

    
