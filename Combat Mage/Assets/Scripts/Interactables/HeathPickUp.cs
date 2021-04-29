using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathPickUp : InteractiveObject 
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
        if (Vector3.Distance(player.transform.position,transform.position)<3.5)
        {
            Debug.Log("testing");
            HealthEventData heath = new HealthEventData(100f);
            player.ChangeHealth.Try(heath);

            Destroy(gameObject);

        }
        base.OnRaycastUpdate(player);

    }

    public override void OnInteractionStart(Player player)
    {
        base.OnInteractionStart(player);
    }

    public override void OnInteractionUpdate(Player player)
    {
        base.OnInteractionUpdate(player);
    }

    public override void OnInteractionEnd(Player player)
    {
        base.OnInteractionEnd(player);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player >();
            HealthEventData heath = new HealthEventData(100f);
            player.ChangeHealth.Try(heath);
            
            Destroy(gameObject);

        }
    }
}
