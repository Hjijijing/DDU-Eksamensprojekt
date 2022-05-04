using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonScript : ParticleScript
{

    public override float charge { get; set; } = 1f;

    protected override float GetMass()
    {
        return AtomUtil.getMass(1, 0, 0);
    }

    protected override void OnPlayerCollision(AtomScript player)
    {
        if (!player.addProton(1, this)) return;
        captured = true;
    }
}
