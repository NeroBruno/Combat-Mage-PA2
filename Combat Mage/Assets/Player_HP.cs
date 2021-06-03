using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HP : MonoBehaviour
{
    // HP variables for damage
    public int maxHP = 100;
    public int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP < 0)
        {
            this.gameObject.GetComponentInChildren<PlayerMovement>().enabled = false;
        }
    }

    public void takeDamage(int damageTaken)
    {
        this.currentHP -= damageTaken;
    }
}
