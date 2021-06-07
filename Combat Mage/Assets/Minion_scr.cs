using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class Minion_scr : MonoBehaviour
{

    public NavMeshAgent gent;
    float rotspeed = 5f, hurtwindup = 0f, acTime, currTime;
    public LayerMask Whatisplayer;
    [HideInInspector] public Transform targetCharacter;
    bool trigger;
    Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        trigger = false;
        targetCharacter = GameObject.Find("Player").transform;
        acTime = 1f;
        currTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
            if (trigger == true)
            {
                gent.destination = targetCharacter.position;
                RotateTowards(targetCharacter);
                if (gent.remainingDistance < 2)
                {
                    hurtwindup += Time.deltaTime;
                    gent.isStopped = true;
                    anim.SetTrigger("MeleeAttack");
                }
                else
                {
                    hurtwindup = 0f;
                    gent.isStopped = false;
                    anim.ResetTrigger("MeleeAttack");
                }

                if (hurtwindup > 2f)
                {
                    hurtPlayer();
                }
            }
            else
            {
                currTime += Time.deltaTime;
                if (currTime > acTime)
                {
                    trigger = true;
                    gent.enabled = true;
                    gent.Warp(this.transform.position);
                }
            }
    }


    void hurtPlayer()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Player_HP>().takeDamage(10);
        Debug.Log("hurting");
        Debug.Log(player.GetComponent<Player_HP>().currentHP);
        hurtwindup = 0f;

    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotspeed);
    }

    public void warpTo( Transform t)
    {
        gent.Warp(t.position);
    }
}
