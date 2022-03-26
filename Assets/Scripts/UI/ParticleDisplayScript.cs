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
    [SerializeField] string format = "$particle: $amount";


    private void Start()
    {
        player.onElectronsAdded += UpdateElectrons;
        player.onNeutronsAdded += UpdateNeutrons;
        player.onProtonsAdded += UpdateProtons;

        UpdateText(protonText, "Protons", player.getProtons());
        UpdateText(neutronText, "Neutrons", player.getNeutrons());
        UpdateText(electronText, "Electrons", player.getElectrons());
    }


    private void OnDestroy()
    {
        player.onElectronsAdded -= UpdateElectrons;
        player.onNeutronsAdded -= UpdateNeutrons;
        player.onProtonsAdded -= UpdateProtons;
    }


    void UpdateProtons(uint before, uint after, uint change, AtomScript atomScript)
    {
        UpdateText(protonText, "Protons", after);

        if (protonAnimation != null)
            protonAnimation.Stop();

        protonAnimation = GetAnimation(protonText);
        protonAnimation.Start();

    }

    void UpdateElectrons(uint before, uint after, uint change, AtomScript atomScript)
    {
        UpdateText(electronText, "Electrons", after);

        if (electronAnimation != null)
            electronAnimation.Stop();

        electronAnimation = GetAnimation(electronText);
        electronAnimation.Start();
    }

    void UpdateNeutrons(uint before, uint after, uint change, AtomScript atomScript)
    {
        UpdateText(neutronText, "Neutrons", after);

        if (neutronAnimation != null)
            neutronAnimation.Stop();

        neutronAnimation = GetAnimation(neutronText);
        neutronAnimation.Start();
    }

    void UpdateText(TextMeshProUGUI field, string particle, uint amount)
    {
        string text = format.Replace("$particle", particle).Replace("$amount", amount.ToString());
        field.text = text;
    }


    TweeningAnimation GetAnimation(TextMeshProUGUI textField)
    {
        TweeningAnimation anim = new TweeningAnimation(this, textField.gameObject);
        anim
            .scale(new Vector3(1.25f, 1.25f, 1.25f), 0.2f)
            .SetEasing(Easing.easeOutSine)
            .rotate(Quaternion.Euler(0, 0, 5f), 0.2f)
            .then()
            .scale(Vector3.one, 0.2f)
            .SetEasing(Easing.easeInSine)
            .rotate(Quaternion.Euler(0, 0, 0), 0.2f);
        return anim;
    }


}
