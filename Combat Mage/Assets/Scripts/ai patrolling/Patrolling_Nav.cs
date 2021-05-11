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
    public Color RadiuscheckColor;
    public TriggerSensor sensor;


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

            break;

            case states.atacking:
                RangeAtack();
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
        if(Vector3.Distance(agent.destination,targetCharacter.position)> 1.0f){
                agent.destination = targetCharacter.position;
        }
    }

    #endregion

    #region atack
    void RangeAtack()
    {
        if (agent.remainingDistance > 10)
        {
            agent.isStopped = false;
            currentState = states.chasing;
        }
        else { 

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

    void Shoot()
    {
        Vector3 shotDirection = targetCharacter.transform.position - Barrel.transform.position;
        GameObject currentShot = Instantiate(projectile, Barrel.transform.position, Quaternion.identity);

        currentShot.GetComponent<Rigidbody>().AddForce(shotDirection.normalized * 10, ForceMode.Impulse);
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
    #endregion
}
