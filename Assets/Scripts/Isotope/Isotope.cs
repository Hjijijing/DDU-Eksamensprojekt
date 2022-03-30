using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="isotope", menuName ="Isotope")]
public class Isotope : ScriptableObject
{
    public int z = 0;
    public int n = 0;
    public string symbol = "";
    public float half_life = 0f;
}
