using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientificConstants : MonoBehaviour
{
    public static ScientificConstants Constants;

    public float G = 6.7f; //Gravitationskonstant
    public float kc = 8f; //Coulomb konstant 
    public float DistanceScale = 1000f; //Hvor meget 1 unity enhed svarer til i fysisk afstand.

    public Color nonMetalColor;
    public Color metalColor;
    public Color nobleGasColor;
    public Color alkaliColor;
    public Color alkaliEarthColor;
    public Color metalloidColor;
    public Color transitionMetalColor;
    public Color halogenColor;
    public Color lanthanideColor;
    public Color actinideColor;
    public Color transactinideColor;
    public Color lockedColor;


    public Color getIsotopeColor(Isotope isotope)
    {
        if (!IsotopeManager.isotopeManager.TryGetCorrespondingElement(isotope, out var element)) 
            return Color.white;
        return getElementColor(element);
    }
    public Color getElementColor(Element element)
    {
        if (element == null) return Color.white;

        switch (element.type)
        {
            case "Nonmetal":
                return nonMetalColor;
            case "Noble Gas":
                return nobleGasColor;
            case "Alkali Metal":
                return alkaliColor;
            case "Alkaline Earth Metal":
                return alkaliEarthColor;
            case "Metalloid":
                return metalloidColor;
            case "Metal":
                return metalColor;
            case "Halogen":
                return halogenColor;
            case "Transition Metal":
                return transitionMetalColor;
            case "Lanthanide":
                return lanthanideColor;
            case "Actinide":
                return actinideColor;
            case "Transactinide":
                return transactinideColor;
            default:
                return Color.white;
        }
    }


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
