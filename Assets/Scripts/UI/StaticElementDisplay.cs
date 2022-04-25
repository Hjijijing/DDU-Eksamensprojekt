using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticElementDisplay : ElementDisplay, IPointerClickHandler
{

    public delegate void ElementPressed(StaticElementDisplay elementDisplay);
    public ElementPressed onElementPressed;

    private Element element;

    public Element Element { get { return element; } set { element = value; UpdateDisplay(); } }

    public void OnPointerClick(PointerEventData eventData)
    {
        onElementPressed?.Invoke(this);
    }

    public void UpdateDisplay()
    {
        setMass(element.atomicMass.ToString());
        setAtomName(element.elementName);
        setSymbol(element.elementSymbol);
        setAtomNumber(element.atomicNumber);
        setElectronConfiguration(element.atomicNumber);
        setColor(element);
    }


    public new void setColor(Element element)
    {
        if (!GameManager.gameManager.elementIsUnlocked(element))
            setColor(ScientificConstants.Constants.lockedColor);
        else
            base.setColor(element);
    }

}
