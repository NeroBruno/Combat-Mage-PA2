using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp_Health : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
        Vector3 mouse = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider != null && hit.collider.CompareTag("Item"))
            {
                Debug.Log("Item Collected");
                Destroy(hit.transform.gameObject);
            }

                if (hit.collider != null && hit.collider.CompareTag("Elemental"))
                {
                    Debug.Log("New Power Unlock");
                    Destroy(hit.transform.gameObject);
                }
            }
        }
       
    }
}
