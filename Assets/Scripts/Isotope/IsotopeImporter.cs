using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class IsotopeImporter : MonoBehaviour
{

    #if UNITY_EDITOR
    public void CreateIsotopeFiles()
    {
        string filePath = Application.dataPath + "/Isotopes/isotopechart.csv";

        string fullString = File.ReadAllText(filePath);

        string[] lines = fullString.Split('\n');

        for(int i = 1; i < lines.Length; i++)
        {
            string savePath = "Assets/Isotopes/isotope" + i + ".asset";

            string line = lines[i];
            string[] values = line.Split(',');

            Isotope isotope = ScriptableObject.CreateInstance<Isotope>();

            isotope.z = int.Parse(values[0]);
            isotope.n = int.Parse(values[1]);
            isotope.symbol = values[2];
            try
            {
            isotope.half_life = float.Parse(values[10]);

            } catch(FormatException e)
            {
                isotope.half_life = 0f;
            }

            AssetDatabase.CreateAsset(isotope, savePath);
        }

        AssetDatabase.SaveAssets();
    }

    
    public void OnGUI()
    {
        if (GUILayout.Button("Create Isotopes"))
        {
            CreateIsotopeFiles();
        }
    }

    #endif

}
