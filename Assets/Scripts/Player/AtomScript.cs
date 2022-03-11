using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtomScript : MonoBehaviour
{
    Vector2 movementDirection = Vector2.zero;

    [SerializeField] int protons = 0;
    [SerializeField] int neutrons = 0;
    [SerializeField] int electrons = 0;


    // Start is called before the first frame update
    void Start()
    {
        
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
        transform.Translate(movementDirection * Time.fixedDeltaTime);
    }


    public void OnTranslation(InputValue value)
    {
        Debug.Log("YALLAH HABIBI");
        movementDirection = value.Get<Vector2>().normalized;
    }
}
