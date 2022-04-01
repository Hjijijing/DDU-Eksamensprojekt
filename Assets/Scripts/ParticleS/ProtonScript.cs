using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonScript : ParticleScript
{

    protected override float GetMass()
    {
        return AtomUtil.getMass(1, 0, 0);
    }

    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addProton(1,this);
        captured = true;
    }
}
