using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class LivingEntityComponent : MonoBehaviour
{
    public LivingEntity Entity
    {
        get
        {
            if (!_Entity)
                _Entity = GetComponent<LivingEntity>();
            if (!_Entity)
                _Entity = GetComponentInParent<LivingEntity>();

            return _Entity;
        }
    }

    private LivingEntity _Entity;

    public virtual void OnEntityStart() { }
}
