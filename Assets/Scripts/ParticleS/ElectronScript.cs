using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronScript : ParticleScript
{



    public override float charge { get; set; } = -1f;

    protected override float GetMass()
    {
        return AtomUtil.getMass(0, 0, 1);
    }


    protected override void OnPlayerCollision(AtomScript player)
    {
        if (!player.addElectron(1, this)) return;
        captured = true;
    }
}
