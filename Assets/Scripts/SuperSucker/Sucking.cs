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

        Vector3 gravitational = GetGravitationalForce();
        Vector3 coulomb = GetCoulombForce();
        Vector3 totalForce = gravitational + coulomb;
        rb.AddForce(totalForce);
    }


    Vector3 GetGravitationalForce()
    {
        float G = ScientificConstants.Constants.G;
        float DistanceScale = ScientificConstants.Constants.DistanceScale;
        Vector3 delta = player.transform.position - transform.position;
        float distanceSquared = delta.sqrMagnitude * Mathf.Pow(DistanceScale, 2);
        Vector3 force = ((G * player.GetMass() * rb.mass) / distanceSquared) * delta.normalized;
        return force;
    }

    Vector3 GetCoulombForce()
    {
        float q1 = ps.charge;
        float q2 = player.getCharge();
        float kc = ScientificConstants.Constants.kc;
        Vector3 delta = transform.position - player.transform.position;
        float DistanceScale = ScientificConstants.Constants.DistanceScale;
        float distanceSquared = delta.sqrMagnitude * Mathf.Pow(DistanceScale, 2);
        Vector3 force = ((kc * q1 * q2) / distanceSquared) * delta.normalized;
        return force;
    }
}

    
