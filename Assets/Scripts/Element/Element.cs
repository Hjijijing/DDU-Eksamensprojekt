using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "isotope", menuName = "Isotope")]
public class Element : ScriptableObject
{
    public string elementName = "";
    public string elementSymbol = "";
    public float atomicMass = 0f;
    public int atomicNumber = 0;
    public int numberOfNeutrons = 0;
    public int period = 0;
    public int group = 0;
    public string phase = "";
    public string type = ""; //Ikke-metal, ædelgas, etc.

}
