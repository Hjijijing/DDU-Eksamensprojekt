using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElementDisplay : ElementDisplay
{
    [SerializeField] AtomScript player;
    bool animate = false;



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
        setColor(atomScript.isotope);
    }

    void ElectronPickup(uint before, uint after, uint change, AtomScript atomScript)
    {
        setElectronConfiguration((int)after);
        setMass(atomScript);
    }

    void NeutronPickup(uint before, uint after, uint change, AtomScript atomScript)
    {
        setMass(atomScript);
        setColor(atomScript.isotope);
    }




}
