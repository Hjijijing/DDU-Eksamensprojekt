using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hjijijing.Tweening;
using TMPro;
using UnityEngine.UI;

public class EndscreenAnimation : MonoBehaviour
{
    [SerializeField] GameObject[] endscreenElements;
    [SerializeField] TextMeshProUGUI titleElement;
    [SerializeField] Image bgImage;
    [SerializeField] Color bgColor;
    public string title;


    private void Start()
    {
        titleElement.text = title;
        bgImage.color = bgColor;
        AnimateIn();
    }


    void AnimateIn()
    {
        var anim = this.Tween(Easing.easeInSine)
            .scale(Vector3.one, 0.5f)
            .from(new Vector3(1, 0, 1));


        foreach (GameObject go in endscreenElements)
        {
            anim.scale(go, Vector3.zero, 0);
        }



        anim.then()
            .UseEasing(Easing.easeOutBounce);


        foreach(GameObject go in endscreenElements)
        {
            anim.scale(go, Vector3.one, 1f)
                .from(Vector3.zero);
        }

        anim.Start();
    }



}
