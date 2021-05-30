using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolState", menuName = "UnityFSM/States/Patrol")]
public class PatrolState : AbstractFSMState
{
    //private ConnectedWaypoint[] _patrolPoints;
    private int _patrolPointIndex;

    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FSMStateType.PATROL;
        _patrolPointIndex = -1;
    }

    public override bool EnterState()
    {
        EnteredState = false;
        if (base.EnterState())
        {
            // Get and store the patrol points
            //_patrolPoints = _npc.PatrolPoints;

            //if (_patrolPoints == null || _patrolPoints.Length == 0)
            //{
            //    Debug.LogError("PatrolState error!");
            //}
            //else
            //{
            //    if (_patrolPointIndex < 0)
            //    {
            //        _patrolPointIndex = Random.Range(0, _patrolPoints.Length);
            //    }
            //    else
            //    {
            //        _patrolPointIndex = (_patrolPointIndex + 1) % _patrolPoints.Length;
            //    }

                _npcMaterial.material.color = Color.blue;
            //    SetDestination(_patrolPoints[_patrolPointIndex]);
                Debug.Log("Entered patrol state");
                EnteredState = true;
        //    }
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            //if (Vector3.Distance(_navMeshAgent.transform.position,
            //        _patrolPoints[_patrolPointIndex].transform.position) <= 1f)
            //{
            //    _fsm.EnterState(FSMStateType.IDLE);
            //}

            if (Vector3.Distance(_navMeshAgent.transform.position, _playerTransform.transform.position) <= 3.5f)
            {
                _fsm.EnterState((FSMStateType.CHASE));
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting patrol state");
        return true;
    }

    //private void SetDestination(ConnectedWaypoint destination)
    //{
    //    if (_navMeshAgent != null && destination != null)
    //    {
    //        _navMeshAgent.SetDestination(destination.transform.position);
    //    }
    //}
}
