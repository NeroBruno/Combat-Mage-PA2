using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine))]
public class NPC : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private FiniteStateMachine _finiteStateMachine;

    //[SerializeField]
    //private ConnectedWaypoint[] _patrolPoints;
    
    public void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _finiteStateMachine = GetComponent<FiniteStateMachine>();
    }

    public void Start()
    {
        
    }

    public void Update()
    {

    }

    //public ConnectedWaypoint[] PatrolPoints
    //{
    //    get => _patrolPoints;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
