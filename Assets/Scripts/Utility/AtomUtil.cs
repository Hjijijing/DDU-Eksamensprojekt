using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomUtil
{
    static int[] shellOrder = new int[] { 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7 };

    static float protonMass = 1f;
    static float neutronMass = 1f;
    static float electronMass = 0.0005f;

    public static int getOuterShell(int electron)
    {
        if (electron < 0 || electron > shellOrder.Length - 1)
            return 0;
        return shellOrder[electron - 1];
    }


    public static float getMass(int protons, int neutrons, int electrons)
    {
        return protons * protonMass + neutrons * neutronMass + electrons * electronMass;
    }


}
