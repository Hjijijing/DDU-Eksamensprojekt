using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementDisplay : MonoBehaviour
{

    [SerializeField] Image backgroundImage;
    [SerializeField] Image borderImage;

    [SerializeField] TextMeshProUGUI mass;
    [SerializeField] TextMeshProUGUI symbol;
    [SerializeField] TextMeshProUGUI atomName;
    [SerializeField] TextMeshProUGUI atomNumber;
    [SerializeField] TextMeshProUGUI electronConfiguration;
    [TextArea]
    [SerializeField] string electronConfigFormat = "$1\n$2\n$3\n$4\n$5\n$6\n$7";



    public void setAtomNumber(int protons)
    {
        setAtomNumber(protons.ToString());
    }

    public void setAtomNumber(string newAtomNumber)
    {
        atomNumber.text = newAtomNumber;
    }


    public void setSymbol(int protonAmount)
    {
        setSymbol(AtomUtil.getAtomSymbol(protonAmount));
    }

    public void setSymbol(string newSymbol)
    {
        symbol.text = newSymbol;
    }



    public void setAtomName(int protons)
    {
        setAtomName(AtomUtil.getAtomName(protons));
    }


    public void setAtomName(string newAtomName)
    {
        atomName.text = newAtomName;
    }


    public void setMass(AtomScript atom)
    {
        setMass(atom.GetMass().ToString());
    }

    public void setMass(string newMass)
    {
        mass.text = newMass;
    }


    public void setElectronConfiguration(int electronAmount)
    {
        setElectronConfiguration(AtomUtil.getShellConfiguration(electronAmount));
    }

    public void setElectronConfiguration(int[] config)
    {
        string result = electronConfigFormat;
        for(int i = 0; i < config.Length; i++)
        {
            string oldValue = "$" + (i + 1);
            string newValue = config[i] == 0 ? "" : config[i].ToString();
            result = result.Replace(oldValue, newValue);
        }

        electronConfiguration.text = result;
    }



}
