using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hjijijing.Tweening;

public class PlayerElementDisplay : ElementDisplay
{
    [SerializeField] AtomScript player;
    [SerializeField]bool animate = false;
    [SerializeField] float animationScale = 1.25f;
    [SerializeField] float animationDuration = 0.4f;
    TweeningAnimation animation;


    private void Start()
    {
        player.onElectronsAdded += ElectronPickup;
        player.onProtonsAdded += ProtonPickup;
        player.onNeutronsAdded += NeutronPickup;

        ProtonPickup(player.getProtons(), player.getProtons(), 0, player);
        ElectronPickup(player.getElectrons(), player.getElectrons(), 0, player);
        NeutronPickup(player.getNeutrons(), player.getNeutrons(), 0, player);
    }


    private void OnDestroy()
    {
        player.onElectronsAdded -= ElectronPickup;
        player.onProtonsAdded -= ProtonPickup;
        player.onNeutronsAdded -= NeutronPickup;
    }



    void ProtonPickup(uint before, uint after, uint change, AtomScript atomScript)
    {
        setSymbol((int)after);
        setAtomName((int)after);
        setAtomNumber((int)after);
        setMass(atomScript);
        setColor((int)atomScript.getProtons());
        if (animate) Animate();
    }

    void ElectronPickup(uint before, uint after, uint change, AtomScript atomScript)
    {
        setElectronConfiguration((int)after);
        setMass(atomScript);
        if (animate) Animate();
    }

    void NeutronPickup(uint before, uint after, uint change, AtomScript atomScript)
    {
        setMass(atomScript);
        setColor((int)atomScript.getProtons());
        if (animate) Animate();
    }


    void Animate()
    {
        animation?.Stop();

        animation = this.Tween(gameObject, Easing.easeInOutSine)
            .scale(new Vector2(animationScale, animationScale), animationDuration)
            .from(Vector3.one)
            .ReturnBack(Easing.easeOutSine);
        animation.Start();
    }




}
