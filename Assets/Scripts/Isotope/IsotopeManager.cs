using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsotopeManager : MonoBehaviour
{

    public static IsotopeManager isotopeManager;

    [SerializeField] Isotope[] isotopes;


    public float highestHalflife = float.MinValue;
    public float lowestHalflife = float.MaxValue;


    private void Awake()
    {
        if(isotopeManager == null)
        {
            isotopeManager = this;
            return;
        }

        Destroy(this);
    }

    private void Start()
    {
        FindHighestAndLowestHalfLife();
    }

    void FindHighestAndLowestHalfLife()
    {
        foreach(Isotope i in isotopes)
        {
            if (i.half_life < lowestHalflife)
                lowestHalflife = i.half_life;
            if (i.half_life > highestHalflife)
                highestHalflife = i.half_life;
        }
    }

    public float MapHalfLife(float halfLife, float minValue, float maxValue)
    {
        return Mathf.Lerp(minValue, maxValue, (halfLife - lowestHalflife) / (highestHalflife - lowestHalflife));
    }

    public Isotope GetIsotope(int protons, int neutrons) 
    { 
        foreach(Isotope i in isotopes)
        {
            if (i.n == neutrons && i.z == protons)
                return i;
        }

        return null;
    }





}
