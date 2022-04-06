using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticElementDisplay : ElementDisplay
{

    private Element element;

    public Element Element { private get { return element; } set { element = value; UpdateDisplay(); } }


    public void UpdateDisplay()
    {
        setMass(AtomUtil.getMass(element.atomicNumber, element.numberOfNeutrons, element.atomicNumber).ToString());
        setAtomName(element.elementName);
        setSymbol(element.elementSymbol);
        setAtomNumber(element.atomicNumber);
        setElectronConfiguration(element.atomicNumber);
        setColor(element);
    }

}
