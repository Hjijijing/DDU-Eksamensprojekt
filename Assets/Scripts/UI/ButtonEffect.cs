using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hjijijing.Tweening;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TweeningAnimation buttonAnimation;
    [SerializeField] float animationDuration = 0.05f;
    [SerializeField] float effect = 1.3f;
    
    
    Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;


    }

    void HoverAnimation()
    {
        buttonAnimation?.Stop();

        Vector3 targetScale = originalScale * effect;
        transform.SetAsLastSibling();

        buttonAnimation = this.Tween(Easing.easeInSine);
        buttonAnimation
            .scale(targetScale, animationDuration)
            .Start();

    }

    void UnHoverAnimation()
    {
        buttonAnimation?.Stop();

        buttonAnimation = this.Tween(Easing.easeInSine);
        buttonAnimation
            .scale(originalScale, animationDuration)
            .Start();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHoverAnimation();
    }

}
