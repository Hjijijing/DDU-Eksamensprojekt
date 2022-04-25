using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using hjijijing.Tweening;

public class ParticleDisplayScript : MonoBehaviour
{
    [SerializeField] AtomScript player;
    [SerializeField] TextMeshProUGUI protonText;
    TweeningAnimation protonAnimation;
    [SerializeField] TextMeshProUGUI neutronText;
    TweeningAnimation neutronAnimation;
    [SerializeField] TextMeshProUGUI electronText;
    TweeningAnimation electronAnimation;
    [SerializeField] string format = "$particle: $amount/$required";


    private void Start()
    {
        player.onElectronsAdded += UpdateElectrons;
        player.onNeutronsAdded += UpdateNeutrons;
        player.onProtonsAdded += UpdateProtons;

        UpdateText(protonText, "Protons", player.getProtons(), GameManager.gameManager.targetElement.atomicNumber);
        UpdateText(electronText, "Electrons", player.getElectrons(), GameManager.gameManager.targetElement.atomicNumber);
        UpdateText(neutronText, "Neutrons", player.getNeutrons(), GameManager.gameManager.targetElement.numberOfNeutrons);
    }


    private void OnDestroy()
    {
        player.onElectronsAdded -= UpdateElectrons;
        player.onNeutronsAdded -= UpdateNeutrons;
        player.onProtonsAdded -= UpdateProtons;
    }


    void UpdateProtons(uint before, uint after, uint change, AtomScript atomScript)
    {
        UpdateText(protonText, "Protons", after, GameManager.gameManager.targetElement.atomicNumber);

        if (protonAnimation != null)
            protonAnimation.Stop();

        protonAnimation = GetAnimation(protonText);
        protonAnimation.Start();

    }

    void UpdateElectrons(uint before, uint after, uint change, AtomScript atomScript)
    {
        UpdateText(electronText, "Electrons", after, GameManager.gameManager.targetElement.atomicNumber);

        if (electronAnimation != null)
            electronAnimation.Stop();

        electronAnimation = GetAnimation(electronText);
        electronAnimation.Start();
    }

    void UpdateNeutrons(uint before, uint after, uint change, AtomScript atomScript)
    {
        UpdateText(neutronText, "Neutrons", after, GameManager.gameManager.targetElement.numberOfNeutrons);

        if (neutronAnimation != null)
            neutronAnimation.Stop();

        neutronAnimation = GetAnimation(neutronText);
        neutronAnimation.Start();
    }

    void UpdateText(TextMeshProUGUI field, string particle, uint amount, int required)
    {
        string text = format.Replace("$particle", particle).Replace("$amount", amount.ToString()).Replace("$required", required.ToString()) ;
        field.text = text;
    }


    TweeningAnimation GetAnimation(TextMeshProUGUI textField)
    {
        TweeningAnimation anim = new TweeningAnimation(this, textField.gameObject);
        anim.scale(Vector3.one, new Vector3(1.25f, 1.25f, 1.25f), 0.4f)
            .ReturnBack()
            .rotate(Quaternion.Euler(0,0,0), Quaternion.Euler(0,0,5f), 0.4f)
            .ReturnBack();

        return anim;
    }


}
