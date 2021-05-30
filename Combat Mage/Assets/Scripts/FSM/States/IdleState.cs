using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "UnityFSM/States/Idle")]
public class IdleState : AbstractFSMState
{
    [SerializeField]
    private float _idleDuration = 3f;

    private float _totalDuration;
    
    public override void OnEnable()
    {
        base.OnEnable();
        StateType = FSMStateType.IDLE;
        
    }

    public override bool EnterState()
    {
        EnteredState = base.EnterState();

        if (EnteredState)
        {
            _npcMaterial.material.color = Color.white;
            Debug.Log("Entered Idle state");
            _totalDuration = 0f;
        }

        return EnteredState;
    }

    public override void UpdateState()
    {
        if (EnteredState)
        {
            _totalDuration += Time.deltaTime;
            
            //Debug.Log("Updating Idle state: " + _totalDuration + " seconds.");
            if (_totalDuration >= _idleDuration)
            {
                _fsm.EnterState((FSMStateType.PATROL));
            }

            if (Vector3.Distance(_navMeshAgent.transform.position, _playerTransform.transform.position) <= 3.5f)
            {
                _fsm.EnterState((FSMStateType.CHASE));
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Idle state");
        return true;
    }
}
