using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : Pickup
{
    protected override void Collect()
    {
        Player.playerInstance.ActivateShields();
        
        base.Collect();
    }
}
