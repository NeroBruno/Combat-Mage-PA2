using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Health_src : MonoBehaviour
{

    // Queremos dar HP ao inimigo, 

    // HP variables for damage
    public int maxHP = 50;
    public int currentHP;
    Patrolling_Nav nav;
    NavMeshAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<Patrolling_Nav>();
        navAgent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP < 0)
        {
            //nav.agent.velocity = new Vector3(0,0,0); // stopping the navigation agent from moving, dies in place
            navAgent.velocity = new Vector3(0, 0, 0); // stopping the navigation agent from moving, dies in place
            Destroy(this.gameObject, 4f);
        }
    }

    public void takeDamage(int damageTaken)
    {
        this.currentHP -= damageTaken;
    }
}
