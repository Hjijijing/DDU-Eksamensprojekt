using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using hjijijing.Tweening;

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



    public void UnLock()
    {
        transform.SetAsLastSibling();
        Quaternion rot1 = Quaternion.Euler(0f, 0f, 30f);
        Quaternion rot2 = Quaternion.Euler(0f, 0f, 0f);
        Quaternion rot3 = Quaternion.Euler(0f, 0f, -30f);
        Vector3 originalScale = transform.localScale;
        Vector3 position = transform.position;

        this.Tween(Easing.easeInOutSine)
            .move(() => { return position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 1f); }, 0.05f)
            .Repeat(38);

        this.Tween(Easing.easeInOutSine)
            .scale(originalScale*1.2f, 0.2f, 0f, 1.8f)
            .then()
            .colorCallback(ScientificConstants.Constants.lockedColor, ScientificConstants.Constants.getElementColor(element), (c)=> { backgroundImage.color = c; }, 0.5f)
            .scale(originalScale*2f, 0.25f)
            .then()
            .scale(originalScale, 0.25f)
            //.scale(originalScale, 1f)
            .then()
            .move(position, 0.05f)
            .Start();
    }

}
