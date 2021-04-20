using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolableObject : MonoBehaviour
{
    public string PoolId { get => _PoolId; }

    public UnityEvent OnReleasedEvent = new UnityEvent();
    public UnityEvent OnUseEvent = new UnityEvent();

    private bool _Initialized;
    private string _PoolId;

    public void Init(string poolId)
    {
        if (_Initialized)
        {
            Debug.LogError("You are attempting to initialize a poolable object but its already initialized!");
            return;
        }

        _PoolId = poolId;
        _Initialized = true;
    }

    public void OnUse()
    {
        OnUseEvent.Invoke();
    }

    public void OnReleased()
    {
        OnReleasedEvent.Invoke();
    }
}
