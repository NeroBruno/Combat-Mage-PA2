using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMove : MonoBehaviour
{

    public Rigidbody Rock;
    public Transform Box1;
    Vector3 Total;
    public float push;
    bool isheld;
    public Transform player;
    //public static bool isMoving;
    static Transform movingRock;


    // Start is called before the first frame update
    void Start()
    {
        //Find where the Riggibody
        Rock = GetComponent<Rigidbody>();
     
        isheld = false;
 
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Hold donw the key P when it closer to the rigybody.It also check is not holing any rocks 
        if (Input.GetKeyDown(KeyCode.P) && (movingRock == null || movingRock == transform) )
        {
            
            Debug.Log(isheld); //Use for testing
            if (!isheld && Vector3.Distance(transform.position, player.position) < 1.5f)  //Checks the distance and see 
            {
                isheld = true;
                //this.Rock.us = false; //Allows to grab and lift objects
                transform.parent = player.transform;
                //Coloca-se o objecto a frente do jogador 
                transform.position = player.position + player.forward * 2;
                //isMoving = true;
                movingRock = transform;
            




            }
            else if(isheld==true)
            {
                
                isheld = false;
                //this.Rock.useGravity = true;
                transform.parent = null;
                //isMoving = false;
                movingRock = null;
              
            }




        }

     

        if(isheld)
        {
            transform.position = player.position + player.forward * 2;
        }

    }

    //Allows to The Box connectes
    private void OnTriggerEnter(Collider collision)
    {
        //See if Box is colliding with the correct swicth.To do so it use a different case(In this case the Tag Puzzle)
        if (collision.CompareTag("Rock"))
        {
            Debug.Log("Experience sucefulll");
        }


    }
}
