using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutronScript : ParticleScript
{
    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addNeutron();
        Destroy(gameObject);
    }
}
