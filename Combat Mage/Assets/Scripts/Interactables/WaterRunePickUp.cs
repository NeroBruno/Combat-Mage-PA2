using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRunePickUp : InteractiveObject
{
    public override void OnInteractionStart(Player player)
    {
        base.OnInteractionStart(player);
        player.HasWaterRune.Set(true);
        Destroy(gameObject);
    }
}
