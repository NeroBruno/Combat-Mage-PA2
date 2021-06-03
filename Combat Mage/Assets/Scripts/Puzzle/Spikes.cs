using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    // This one is not working atm just because the healthpickup object does not have a collider
    private void OnTriggerStay(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            HealthEventData heath = new HealthEventData(-0.1f);
            player.ChangeHealth.Try(heath);

         
        }
    }

}
