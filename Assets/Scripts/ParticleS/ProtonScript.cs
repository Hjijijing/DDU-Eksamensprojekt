using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonScript : ParticleScript
{
    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addProton(1,this);
        captured = true;
    }
}
