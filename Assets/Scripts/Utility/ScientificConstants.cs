using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientificConstants : MonoBehaviour
{
    public static ScientificConstants Constants;

    public float G = 6.7f;

    private void Awake()
    {
        if(Constants == null)
        {
            Constants = this;
            return;
        }

        Destroy(this);
    }


}
