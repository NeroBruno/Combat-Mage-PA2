using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : InteractiveObject 
{
    public override void OnInteractionStart(Player player)
    {
        base.OnInteractionStart(player);
        HealthEventData health = new HealthEventData(100f);
        player.ChangeHealth.Try(health);
        Destroy(gameObject);
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
