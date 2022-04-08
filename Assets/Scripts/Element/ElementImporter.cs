using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class ElementImporter : MonoBehaviour
{

#if UNITY_EDITOR
    public void CreateElementFiles()
    {
        string filePath = Application.dataPath + "/Elements/elements.csv";

        string fullString = File.ReadAllText(filePath);

        string[] lines = fullString.Split('\n');

        for (int i = 1; i < lines.Length-1; i++)
        {
            string savePath = "Assets/Elements/element" + i + ".asset";

            string line = lines[i];
            string[] values = line.Split(',');

            Element element = ScriptableObject.CreateInstance<Element>();

            try {
            element.elementName = values[1];
            element.elementSymbol = values[2];
            element.phase = values[9];
            element.type = values[15];
            element.atomicMass = float.Parse(values[3].Replace(".",","));
            element.atomicNumber = int.Parse(values[0]);
            element.numberOfNeutrons = int.Parse(values[4]);
            element.period = int.TryParse(values[7], out int period) ? period : -1;
            element.group = int.TryParse(values[8], out int group) ? group: -1;
            AssetDatabase.CreateAsset(element, savePath);

            } catch(FormatException e)
            {
                Debug.Log("Import error on csv line" + (i + 1));
                Debug.LogError(e);
            }

            
        }

        AssetDatabase.SaveAssets();
    }


    public void OnGUI()
    {
        if (GUILayout.Button("Create Elements"))
        {
            CreateElementFiles();
        }
    }

#endif

}
