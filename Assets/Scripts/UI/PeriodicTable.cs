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
    [SerializeField] bool squareAspect = true;
    [SerializeField] bool useBoxSize = false;




    // Start is called before the first frame update
    void Start()
    {
        CreateTable();
    }

    void CreateTable()
    {
        Vector2 elementSizeInUnits = ((RectTransform)elementPrefab.transform).sizeDelta;

        Vector2Int groupRange = new Vector2Int(int.MaxValue, int.MinValue);
        Vector2Int periodRange = new Vector2Int(int.MaxValue, int.MinValue);

        foreach (Element element in elements)
        {
            if (element.group < groupRange.x) groupRange.x = element.group;
            if (element.group > groupRange.y) groupRange.y = element.group;
            if (element.period < periodRange.x) periodRange.x = element.period;
            if (element.period > periodRange.y) periodRange.y = element.period;
        }

        int groupAmount = groupRange.y - groupRange.x + 1;
        int periodAmount = periodRange.y - periodRange.x + 1;

        float tableWidth = size.x;
        float tableHeight = size.y;

        if(useBoxSize)
        {
            RectTransform rt = ((RectTransform)transform);
            tableWidth = rt.sizeDelta.x;
            tableHeight = rt.sizeDelta.y;
        }

        float elementSizeX = (tableWidth - ((margin.x + margin.z) + (groupAmount - 1) * spacing.x)) / groupAmount;
        float elementSizeY = (tableHeight - ((margin.y + margin.w) + (periodAmount - 1) * spacing.y)) / periodAmount;

        if (squareAspect)
        {
            if (elementSizeX > elementSizeY)
            {
                elementSizeX = elementSizeY;
            } else
            {
                elementSizeY = elementSizeX;
            }
        }

        float scaleX = elementSizeX / elementSizeInUnits.x;
        float scaleY = elementSizeY / elementSizeInUnits.y;

        foreach (Element element in elements)
        {
            int x = element.group - 1;
            int y = element.period - 1;

            float posX = margin.x + spacing.x * x + elementSizeX * x;
            float posY = -(margin.y + spacing.y * y + elementSizeY * y);

            GameObject elementObject = Instantiate(elementPrefab, transform);
            elementObject.AddComponent<ButtonEffect>();

            RectTransform rt = elementObject.transform as RectTransform;
            rt.anchoredPosition = new Vector2(posX, posY);
            elementObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);

            Vector3 deltaPos = new Vector3((-0.5f) * elementSizeX, 0.5f * elementSizeY);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localPosition -= deltaPos;

            if (elementObject.TryGetComponent(out StaticElementDisplay display))
            {
                display.Element = element;
            }
        }
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        if(GUILayout.Button("Reload Table")){
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            CreateTable();
        }
    }
#endif 

}
