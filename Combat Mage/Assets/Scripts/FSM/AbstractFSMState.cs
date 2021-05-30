using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutionState
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public enum FSMStateType
{
    IDLE,
    PATROL,
    CHASE,
};

public abstract class AbstractFSMState : ScriptableObject
{
    public ExecutionState ExecutionState { get; protected set; }
    public bool EnteredState { get; protected set; }
    public FSMStateType StateType { get; protected set; }

    protected NavMeshAgent _navMeshAgent;
    protected NPC _npc;
    protected FiniteStateMachine _fsm;
    protected Transform _playerTransform;
    protected Renderer _npcMaterial;
    public virtual void OnEnable()
    {
        ExecutionState = ExecutionState.NONE;
    }

    public virtual bool EnterState()
    {
        bool successNavMesh = true;
        bool successNPC = true;
        
        ExecutionState = ExecutionState.ACTIVE;
        // Does the navmeshagent exist?
        successNavMesh = (_navMeshAgent != null);
        // Does the executing agent exist?
        successNPC = (_npc != null);
        return successNavMesh && successNPC;
    }

    public abstract void UpdateState();

    public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.COMPLETED;
        return true;
    }

    public virtual void SetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent != null)
        {
            _navMeshAgent = navMeshAgent;
        }
    }

    public virtual void SetExecutingFSM(FiniteStateMachine fsm)
    {
        if (fsm != null)
        {
            _fsm = fsm;
        }
    }
    
    public virtual void SetExecutingNPC(NPC npc)
    {
        if (npc != null)
        {
            _npc = npc;
        }
    }
    public virtual void SetPlayerTransform(Transform player)
    {
        if (player != null)
        {
            _playerTransform = player;
        }
    }

    public virtual void SetMaterial(Renderer material)
    {
        if (material != null)
        {
            _npcMaterial = material;
        }
    }
}
