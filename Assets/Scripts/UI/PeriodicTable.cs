using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicTable : MonoBehaviour
{

    [SerializeField] Element[] elements;
    [SerializeField] Vector2 spacing;
    [SerializeField] Vector2 size;
    [SerializeField] Quaternion margin;
    [SerializeField] GameObject elementPrefab;




    // Start is called before the first frame update
    void Start()
    {
        foreach(Element element in elements)
        {
            int x = element.group;
            int y = element.period;

            float posX = margin.x + spacing.x * x;
            float posY = -(margin.y + spacing.y * y);

            GameObject elementObject = Instantiate(elementPrefab, transform);

            ((RectTransform)elementObject.transform).anchoredPosition = new Vector2(posX, posY);
            
            if(elementObject.TryGetComponent(out StaticElementDisplay display))
            {
                display.Element = element;
            }
        }
    }

    
}
