using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Health_src : MonoBehaviour
{

    // Queremos dar HP ao inimigo, 

    // HP variables for damage
    public int maxHP = 50;
    private int currentHP;

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
            Destroy(this.gameObject);
        }
    }

    public void takeDamage(int damageTaken)
    {
        this.currentHP -= damageTaken;
    }
}
