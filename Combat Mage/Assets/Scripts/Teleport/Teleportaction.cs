using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportaction : MonoBehaviour
{
    public Transform teleport;
    public GameObject player;
    public GameObject box;

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleport.transform.position;
    }
}
