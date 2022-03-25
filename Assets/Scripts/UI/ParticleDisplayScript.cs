using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleDisplayScript : MonoBehaviour
{
    [SerializeField] AtomScript player;
    [SerializeField] Text text;



    private void Update()
    {
        text.text = "Protons: " + player.getProtons() + "\nNeutrons: " + player.getNeutrons() + "\nElectrons: " + player.getElectrons();
    }
}
