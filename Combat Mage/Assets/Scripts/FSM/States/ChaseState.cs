using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ChaseState", menuName = "UnityFSM/States/Chase")]
public class ChaseState : AbstractFSMState
{
    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FSMStateType.CHASE;
    }

    public override bool EnterState()
    {
        EnteredState = false;
        if (base.EnterState())
        {
            if (_playerTransform == null)
            {
                Debug.LogError("No player transform!");
            }
            else
            {
                _npcMaterial.material.color = Color.red;
                SetDestination(_playerTransform);
                EnteredState = true;
                Debug.Log("Entered Chase state");
            }
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            SetDestination(_playerTransform);
        }

        if (Vector3.Distance(_navMeshAgent.transform.position, _playerTransform.transform.position) > 3.5f)
        {
            _fsm.EnterState((FSMStateType.IDLE));
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("Exited chase state.");
        return true;
    }

    private void SetDestination(Transform player)
    {
        if (_navMeshAgent != null && player != null)
        {
            _navMeshAgent.SetDestination(player.transform.position);
        }
    }
}
