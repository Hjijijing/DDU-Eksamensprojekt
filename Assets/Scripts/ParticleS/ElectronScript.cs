using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronScript : ParticleScript
{
    protected override void OnPlayerCollision(AtomScript player)
    {
        player.addElectron(1,this);
        captured = true;
    }
}
