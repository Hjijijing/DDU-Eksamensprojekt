using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public abstract class ParticleScript : MonoBehaviour
{
    [SerializeField] float maxInitialForce = 1000f;
    public bool captured = false;
    public abstract float charge { get; set; }

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 force = new Vector2(Random.Range(-maxInitialForce, maxInitialForce), Random.Range(-maxInitialForce, maxInitialForce));
        rb.AddForce(force, ForceMode2D.Impulse);

        rb.mass = GetMass();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!GameManager.gameManager.isRunning) return;

        if (captured) return;

        if(collider.gameObject.TryGetComponent<AtomScript>(out AtomScript player))
        {
            OnPlayerCollision(player);
        }
    }

    protected abstract float GetMass();
    protected abstract void OnPlayerCollision(AtomScript player);

}
