using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucking : MonoBehaviour
{
    public GameObject player;
    public float G = 6.7f;
    Rigidbody2D rb;



    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        Vector3 delta = player.transform.position - transform.position;
        Vector3 force = ((G * 1f * 1f) / delta.sqrMagnitude) * delta.normalized;



        rb.AddForce(force);


    }
}

    
