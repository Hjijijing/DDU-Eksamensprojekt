using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronScript : ParticleScript
{
    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addElectron();
        Destroy(gameObject);
    }
}
