using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRunePickUp : InteractiveObject
{
    public override void OnInteractionStart(Player player)
    {
        base.OnInteractionStart(player);
        player.HasEarthRune.Set(true);
        Destroy(gameObject);
    }
}
