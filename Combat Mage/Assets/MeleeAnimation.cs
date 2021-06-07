using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAnimation : MonoBehaviour
{

    Animator anim;
    Patrolling_Nav goblin;
    NavMeshAgent minion;
    Transform player;
    bool meleeAttack;

    // Start is called before the first frame update
    void Start()
    {
        meleeAttack = false;
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        goblin = GetComponent<Patrolling_Nav>();
        minion = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goblin.currentState.Equals(null) && minion.remainingDistance < 5)
        {
            anim.SetBool("hitOnce", true);
        }
        // Dps do Goblin morrer, isto dá erro. Como corrigir?
        else if (goblin.currentState.Equals(Patrolling_Nav.states.chasing))
            {
                anim.SetBool("spotPlayer", true);

            }


        // Como usar o MeleeAttack corretamente? Como chamar aqui a animação apenas qd o ataque é executado?

        //if (goblin.currentState.Equals(Patrolling_Nav.states.atacking))
        //{
        //if (Vector3.Distance(player.position, goblin.agent.destination) <= 2)
        //{
        //    anim.SetTrigger("MeleeAttack");
        //}
        //}

    }
}
