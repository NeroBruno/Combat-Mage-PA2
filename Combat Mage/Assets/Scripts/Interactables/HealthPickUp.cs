using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : InteractiveObject 
{
    public override void OnRaycastStart(Player player)
    {
        base.OnRaycastStart(player);
    }

    public override void OnRaycastEnd(Player player)
    {
        base.OnRaycastEnd(player);
    }

    public override void OnRaycastUpdate(Player player)
    {
        base.OnRaycastUpdate(player);
    }

    public override void OnInteractionStart(Player player)
    {
        base.OnInteractionStart(player);
        HealthEventData health = new HealthEventData(100f);
        player.ChangeHealth.Try(health);
        Destroy(gameObject);
    }

    public override void OnInteractionUpdate(Player player)
    {
        base.OnInteractionUpdate(player);
    }

    public override void OnInteractionEnd(Player player)
    {
        base.OnInteractionEnd(player);
    }

    // This one is not working atm just because the healthpickup object does not have a collider
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            HealthEventData heath = new HealthEventData(100f);
            player.ChangeHealth.Try(heath);

            Destroy(gameObject);
        }
    }
}
