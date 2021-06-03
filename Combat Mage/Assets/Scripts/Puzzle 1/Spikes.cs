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

    private void OnTriggerEnter(Collider collision)
    {
        //See if Box is colliding with the correct swicth.To do so it use a different case(In this case the Tag Puzzle)
        if (collision.CompareTag("Spikes"))
        {
            Debug.Log("Experience sucefulll");
        }

    }
}
