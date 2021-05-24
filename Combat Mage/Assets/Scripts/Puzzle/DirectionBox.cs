using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionBox : MonoBehaviour
{
    public Rigidbody RigiBox1;
    public Transform Box1;
    Vector3 Total;
    public float push;
    bool isheld;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //Find where the Riggibody
        RigiBox1 = GetComponent<Rigidbody>();
        isheld = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
          //Hold donw the key E when it closer to the rigybody.
        if(Input.GetKeyDown(KeyCode.E))
        {
            #region comment
            
            ////Condiction if moves up 
            //if(Input.GetKey(KeyCode.W))
            //{

            ////Calcula a posição da Box e mtipluca a velocidade(velucidade=push).It used transform foward in order to move acording to the orientaction of the Box
            //Total = Box1.position + transform.forward*Time.fixedDeltaTime*push;

            // RigiBox1.MovePosition(Total);

            //}

            ////Condiction if moves right 
            //else if ((Input.GetKey(KeyCode.A)))
            //{
            //    Total = Box1.position - transform.right * Time.fixedDeltaTime * push;

            //    RigiBox1.MovePosition(Total);
            //}

            ////Condiction if moves left 
            //else if ((Input.GetKey(KeyCode.D)))
            //{
            //    Total = Box1.position + transform.right * Time.fixedDeltaTime * push;

            //    RigiBox1.MovePosition(Total);
            //}
            #endregion

            //if (!isheld)
            //{
            //    isheld = true;
                //this.RigiBox1.useGravity = false; //Allows to grab and lift objects

            //Allows to move the player while 
                transform.parent = player.transform;
            //}
         //else
         //   {
                //isheld = false;
                //this.RigiBox1.useGravity = true;
        
            //}
         
         
          
            
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            transform.parent = null;
        }
    }

    //Allows to The Box connectes
    private void OnTriggerEnter(Collider collision)
    {
        //See if Box is colliding with the correct swicth.To do so it use a different case(In this case the Tag Puzzle)
        if(collision.CompareTag("Puzzle"))
        {
            Debug.Log("Experience sucefulll");
        }

        //Same as 
        if(collision.CompareTag("Puzzle2"))
        {
            Debug.Log("Experience sucefulll");
        }

        //See if Box is colliding with the correct swicth.To do so it use a different case(In this case the Tag Puzzle)
        if (collision.CompareTag("Spikes"))
        {
            Debug.Log("Taking Damage");
        }
    }

    
}
    
      
