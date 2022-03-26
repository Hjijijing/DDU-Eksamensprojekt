using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [SerializeField] float parallaxFactor = 0.5f;
    [SerializeField] GameObject observer;
    Vector3 lastObserverPosition;
    Vector2 spriteSizeInUnits;

    void Start()
    {
        lastObserverPosition = observer.transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;
        Texture2D texture = sprite.texture;
        float ppu = sprite.pixelsPerUnit;

        float sizeX = texture.width / ppu;
        float sizeY = texture.height / ppu;

        spriteSizeInUnits = new Vector2(sizeX, sizeY);
    }

   //LateUpdate så det bliver kaldt efter observeren har bevæget sig
    void LateUpdate()
    {
        Vector3 newObserverPosition = observer.transform.position;
        Vector3 deltaObserver = newObserverPosition - lastObserverPosition;
        if (deltaObserver == Vector3.zero) return;

        Vector3 deltaMovement = deltaObserver * parallaxFactor;

        Vector3 newPosition = transform.position + deltaMovement;

        transform.position = newPosition;


        Vector3 positionDifference = newObserverPosition - transform.position;

        if(Mathf.Abs(positionDifference.x) >= spriteSizeInUnits.x)
        {
            float offset = positionDifference.x % spriteSizeInUnits.x;
            transform.position = new Vector3(newObserverPosition.x + offset, transform.position.y, transform.position.z);
        }

        if (Mathf.Abs(positionDifference.y) >= spriteSizeInUnits.y)
        {
            float offset = positionDifference.y % spriteSizeInUnits.y;
            transform.position = new Vector3(transform.position.x, newObserverPosition.y, transform.position.z);
        }



        lastObserverPosition = newObserverPosition;
    }
}
