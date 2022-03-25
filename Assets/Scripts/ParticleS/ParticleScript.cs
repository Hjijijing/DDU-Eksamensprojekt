using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public abstract class ParticleScript : MonoBehaviour
{


   
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent<AtomScript>(out AtomScript player))
        {
            OnPlayerCollision(player);
        }
    }


    protected abstract void OnPlayerCollision(AtomScript player);

}
