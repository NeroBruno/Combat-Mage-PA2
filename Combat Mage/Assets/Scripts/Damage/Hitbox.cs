using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Will register external damage events and pass them to the parent entity
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Hitbox : MonoBehaviour, IDamageable
{
    #region Internal
    [Serializable]
    public class DamageEvent : UnityEvent<HealthEventData> { }

    [Serializable]
    public class DamageEventSimple : UnityEvent<float> { }
    #endregion

    public Collider Collider { get { return _Collider; } }

    [SerializeField]
    //[Clamp(0f, Mathf.Infinity)]
    private float _DamageMultiplier = 1f;

    //[SerializeField]
    //private SoundPlayer _GroundImpactSound = null;

    [SerializeField]
    private DamageEvent _OnDamageEvent = null;

    [SerializeField]
    private DamageEventSimple _OnDamageEventSimple = null;

    private Collider _Collider;
    private Rigidbody _Rigidbody;
    private LivingEntity _ParentEntity;

    private bool _HitboxImpact;

    public void TakeDamage(HealthEventData damageData)
    {
        if (enabled)
        {
            _OnDamageEvent.Invoke(damageData);
            _OnDamageEventSimple.Invoke(damageData.Delta);

            if(_ParentEntity != null)
            {
                if(_ParentEntity.Health.Get() > 0f)
                {
                    damageData.Delta *= _DamageMultiplier;
                    //Debug.Log(_ParentEntity.Health.Get().ToString());
                    _ParentEntity.ChangeHealth.Try(damageData);
                }

                if (_ParentEntity.Health.Get() == 0f)
                {
                    _Rigidbody.AddForceAtPosition(damageData.HitDirection * damageData.HitImpulse, damageData.Hitpoint, ForceMode.Impulse);
                    
                }
            }
        }
    }

    public LivingEntity GetEntity()
    {
        return _ParentEntity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.sqrMagnitude > 5f && !_Rigidbody.isKinematic && !_HitboxImpact)
        {
            //_GroundImpactSound.PlayAtPosition(ItemSelection.Method.RandomExcludeLast, transform.position, 1f);
            _HitboxImpact = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            HealthEventData projectileDamageData = new HealthEventData(-25f, DamageType.Generic, transform.position, (other.gameObject.transform.position - transform.position).normalized, 50f, Vector3.zero);
            TakeDamage(projectileDamageData);
            Debug.Log(_ParentEntity.name + " took a hit");
            Debug.Log(_ParentEntity.Health.Get().ToString());

            Destroy(other.gameObject);
        }
    }

    private void Awake()
    {
        _ParentEntity = GetComponentInParent<LivingEntity>();

        _Collider = GetComponent<Collider>();
        _Rigidbody = GetComponent<Rigidbody>();

        _ParentEntity.Respawn.AddListener(Respawn);
    }

    private void Respawn()
    {
        _HitboxImpact = false;
    }
}
