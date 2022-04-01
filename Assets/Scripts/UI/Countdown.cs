using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using hjijijing.Tweening;

public class Countdown : MonoBehaviour
{
    public float StartAnimationDuration = 0.2f;
    Image fillbar;
    public float x = 0f, y = 0f;
    public Color color = Color.white;


    private void Awake()
    {
        fillbar = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        new TweeningAnimation(this, gameObject, Easing.easeInOutSine)
            .scale(Vector3.one, StartAnimationDuration)
            .from(Vector3.zero)
            .Start();

        ((RectTransform)transform).anchoredPosition = new Vector2(x, y);
        fillbar.color = color;
    }

    public void SetFill(float amount)
    {
        fillbar.fillAmount = amount;
    }

    public void Remove()
    {
        new TweeningAnimation(this, gameObject, Easing.easeInOutSine)
            .scale(Vector3.zero, StartAnimationDuration)
            .from(Vector3.one)
            .then()
            .call(() => { Destroy(gameObject); })
            .Start();
        
    }

}
