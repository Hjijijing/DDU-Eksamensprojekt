using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucking : MonoBehaviour
{
    public AtomScript player;
    public ParticleScript ps;
    Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<AtomScript>();
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleScript>();
    }

    void FixedUpdate()
    {
        if (ps.captured) return;
        float G = ScientificConstants.Constants.G;
        float DistanceScale = ScientificConstants.Constants.DistanceScale;
        Vector3 delta = player.transform.position - transform.position;
        Vector3 force = ((G * player.GetMass() * rb.mass) / (delta.sqrMagnitude*Mathf.Pow(DistanceScale,2))) * delta.normalized;

        rb.AddForce(force);
    }
}

    
