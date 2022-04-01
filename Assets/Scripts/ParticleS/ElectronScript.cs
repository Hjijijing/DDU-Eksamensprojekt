using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronScript : ParticleScript
{

    protected override float GetMass()
    {
        return AtomUtil.getMass(0, 0, 1);
    }


    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addElectron(1,this);
        captured = true;
    }
}
