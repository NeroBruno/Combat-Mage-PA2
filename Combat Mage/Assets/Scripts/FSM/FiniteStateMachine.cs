using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniteStateMachine : MonoBehaviour
{
    //    [SerializeField] 
    //    private AbstractFSMState _startingState;
    public Transform playerTransform;
    private AbstractFSMState _currentState;

    [SerializeField]
    private List<AbstractFSMState> _validStates;
    private Dictionary<FSMStateType, AbstractFSMState> _fsmStates;
    public void Awake()
    {
        _currentState = null;
        
        _fsmStates = new Dictionary<FSMStateType, AbstractFSMState>();

        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        NPC npc = GetComponent<NPC>();
        Transform player = playerTransform;
        Renderer material = GetComponent<MeshRenderer>();
        
        foreach (var state in _validStates)
        {
            state.SetExecutingFSM(this);
            state.SetExecutingNPC(npc);
            state.SetNavMeshAgent(navMeshAgent);
            state.SetPlayerTransform(player);
            state.SetMaterial(material);
            _fsmStates.Add(state.StateType, state);
        }
    }

    public void Start()
    {
        EnterState(FSMStateType.IDLE);
//        if (_startingState != null)
//        {
//            EnterState(_startingState);
//        }
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    public void EnterState(AbstractFSMState nextState)
    {
        if (nextState == null)
            return;

        if (_currentState != null)
        {
            _currentState.ExitState();
        }
        
        _currentState = nextState;
        _currentState.EnterState();
    }

    public void EnterState(FSMStateType stateType)
    {
        if (_fsmStates.ContainsKey(stateType))
        {
            AbstractFSMState nextState = _fsmStates[stateType];
            
            EnterState(nextState);
        }
    }
}
