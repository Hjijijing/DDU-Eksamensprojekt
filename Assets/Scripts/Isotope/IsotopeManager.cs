using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsotopeManager : MonoBehaviour
{

    public enum PICKUP_FOR_STABLE
    {
        PROTON,
        NEUTRON,
        ELECTRON,
        CORE,
        ANY
    }

    public static IsotopeManager isotopeManager;

    [SerializeField] Isotope[] isotopes;
    [SerializeField] Element[] elements;


    public float highestHalflife = float.MinValue;
    public float lowestHalflife = float.MaxValue;



    
    public Element GetCorrespondingElement(Isotope isotope)
    {
        if (isotope == null) return null;
        foreach(Element element in elements)
        {
           if (element.atomicNumber == isotope.z) return element;
        }
        return null;
    }

    public bool TryGetCorrespondingElement(Isotope isotope, out Element element)
    {
        element = GetCorrespondingElement(isotope);
        return element != null;
    }



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
            if (i.half_life == 0f) continue;
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



    public PICKUP_FOR_STABLE findPickupForCore(uint protons, uint neutrons)
    {
        bool pFound = false;
        bool nFound = false;

        int pSteps = 1;
        int nSteps = 1;

        Isotope i;

        while((i = GetIsotope((int)(protons + pSteps), (int)neutrons)) != null)
        {
            if (i.half_life == 0) { pFound = true; break; }
            pSteps++;
        }

        while ((i = GetIsotope((int)protons, (int)(neutrons+nSteps))) != null)
        {
            if (i.half_life == 0) { nFound = true; break; }
            nSteps++;
        }

        if (!pFound && !nFound) return PICKUP_FOR_STABLE.CORE;
        else if (pFound && !nFound) return PICKUP_FOR_STABLE.PROTON;
        else if (!pFound && nFound) return PICKUP_FOR_STABLE.NEUTRON;
        else
        {
            if (pSteps < nSteps) return PICKUP_FOR_STABLE.PROTON;
            else if (pSteps > nSteps) return PICKUP_FOR_STABLE.NEUTRON;
            else return PICKUP_FOR_STABLE.CORE;
        }

    }

}
