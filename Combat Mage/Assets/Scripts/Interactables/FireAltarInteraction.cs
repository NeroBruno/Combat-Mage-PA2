using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAltarInteraction : InteractiveObject
{
    public override void OnInteractionStart(Player player)
    {
        if (player.HasFireRune.Get())
        {
            base.OnInteractionStart(player);
            // Code to complete objective
            // And change something in the game object itself
            // Test
            Destroy(gameObject);
        }

    }
}
