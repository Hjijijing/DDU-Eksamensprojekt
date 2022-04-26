using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutronScript : ParticleScript
{

    public override float charge { get; set; } = 0;

    protected override float GetMass()
    {
        return AtomUtil.getMass(0, 1, 0);
    }

    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addNeutron(1,this);
        captured = true;
    }
}
