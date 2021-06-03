using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SensorToolkit;

public class Patrolling_Nav : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] MoveDestinations;
    [HideInInspector] public Transform targetCharacter;
    public Transform Barrel;
    int currentPatroldestination;
    public RaycastHit Los;
    float TimetoShoot,ShotCd = 5f;
    public LayerMask Whatisplayer;
    public TriggerSensor sensor;
    float rotspeed = 5f,hurtwindup =0f;
    [SerializeField] bool isMelee;


    public GameObject projectile;

    public float radius = 5f;
    
    enum states
    {
        patrolling,
        chasing,
        atacking
    }

    states currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState  = states.patrolling;

        currentPatroldestination = 0;
        agent.SetDestination(MoveDestinations[0].position);


        targetCharacter = GameObject.Find("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case states.patrolling:

                if(agent.remainingDistance < 1)
                {
                        CycleDestinations();
                }
                
                var detected = sensor.GetNearest();
                if (detected != null)
                {
                    FoundOnPatrol();
                }
            break;

            case states.chasing:

                Chase();
                if(agent.remainingDistance < 15)
                {
                    CloseEnough();
                }

            break;

            case states.atacking:
                
                if (isMelee)
                {
                    MeleeAtack();
                }
                else
                {
                    RangeAtack();
                }
            break;


            default:
                Debug.Log("Erro na seleção dos estados");
            break;
        }


        
    }

    #region  patroll
    void CycleDestinations(){
        if(currentPatroldestination < (MoveDestinations.Length-1)){
            currentPatroldestination ++;
            agent.SetDestination(MoveDestinations[currentPatroldestination].position);
        }else{
            currentPatroldestination = 0;
             agent.SetDestination(MoveDestinations[currentPatroldestination].position);
        }
    }



    #endregion

    #region chase
    void Chase(){
        //atualizar onde o ai vai tentar ir
        if(Vector3.Distance(agent.destination,targetCharacter.position) > 1.0f){
                agent.destination = targetCharacter.position;
        }
    }

    #endregion

    #region atack
    void RangeAtack()
    {
        agent.destination = targetCharacter.position;
        if (agent.remainingDistance > 20f)
        {
            agent.isStopped = false;
            currentState = states.chasing;
            Debug.Log("a  tentar dar trigger a chase");
        }
        else {
            RotateTowards(targetCharacter);
        //ver se está perto demais
        if (agent.remainingDistance < 8)
            {
                agent.isStopped = true;
            }

            if(TimetoShoot <= 0f){ 
                Shoot(); 
            }else{
                TimetoShoot -= Time.deltaTime;
            }        
        }
    }

    void MeleeAtack()
    {
        agent.destination = targetCharacter.position;
        RotateTowards(targetCharacter);
        if(agent.remainingDistance < 2)
        {
            hurtwindup += Time.deltaTime;
            agent.isStopped = true;
        }
        else
        {
            hurtwindup = 0f;
            agent.isStopped = false;
        }

        if(hurtwindup > 2f)
        {
            hurtPlayer();
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

    void Shoot()
    {
        Vector3 shotDirection = targetCharacter.transform.position - Barrel.transform.position;
        GameObject currentShot = Instantiate(projectile, Barrel.transform.position, Quaternion.identity);

        currentShot.GetComponent<Rigidbody>().AddForce(shotDirection.normalized * 30, ForceMode.Impulse);
        TimetoShoot = ShotCd;
    }

    #endregion

    #region detection

    public void FoundOnPatrol()
    {
        currentState = states.chasing;
    }

    public void CloseEnough()
    {
        currentState = states.atacking;
    }


    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotspeed);
    }

    #endregion
}
