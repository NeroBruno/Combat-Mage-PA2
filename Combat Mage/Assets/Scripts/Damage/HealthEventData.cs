using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Generic,
    Fire,
    Air,
    Earth,
    Water,
    //Physical
}

/// <summary>
/// 
/// </summary>
public struct HealthEventData
{
    public float Delta { get; set; }

    public LivingEntity Source { get; private set; }

    public DamageType DamageType { get; private set; }

    public Vector3 Hitpoint { get; private set; }

    public Vector3 HitDirection { get; private set; }

    public float HitImpulse { get; private set; }

    public Vector3 HitNormal { get; private set; }

    public HealthEventData(float delta, LivingEntity source = null)
    {
        Delta = delta;

        DamageType = DamageType.Generic;

        Hitpoint = Vector3.zero;
        HitDirection = Vector3.zero;
        HitImpulse = 0f;

        HitNormal = Vector3.zero;

        Source = source;
    }

    public HealthEventData(float delta, DamageType damageType, LivingEntity source = null)
    {
        Delta = delta;

        DamageType = damageType;

        Hitpoint = Vector3.zero;
        HitDirection = Vector3.zero;
        HitImpulse = 0f;

        HitNormal = Vector3.zero;

        Source = source;
    }

    public HealthEventData(float delta, Vector3 hitpoint, Vector3 hitDirection, float hitImpulse, LivingEntity source = null)
    {
        Delta = delta;

        DamageType = DamageType.Generic;

        Hitpoint = hitpoint;
        HitDirection = hitDirection;
        HitImpulse = hitImpulse;

        HitNormal = Vector3.zero;

        Source = source;
    }

    public HealthEventData(float delta, DamageType damageType, Vector3 hitpoint, Vector3 hitDirection, float hitImpulse, LivingEntity source = null)
    {
        Delta = delta;

        DamageType = damageType;

        Hitpoint = hitpoint;
        HitDirection = hitDirection;
        HitImpulse = hitImpulse;

        HitNormal = Vector3.zero;

        Source = source;
    }

    public HealthEventData(float delta, Vector3 hitpoint, Vector3 hitDirection, float hitImpulse, Vector3 hitNormal, LivingEntity source = null)
    {
        Delta = delta;

        DamageType = DamageType.Generic;

        Hitpoint = hitpoint;
        HitDirection = hitDirection;
        HitImpulse = hitImpulse;

        HitNormal = hitNormal;

        Source = source;
    }

    public HealthEventData(float delta, DamageType damageType, Vector3 hitpoint, Vector3 hitDirection, float hitImpulse, Vector3 hitNormal, LivingEntity source = null)
    {
        Delta = delta;

        DamageType = damageType;

        Hitpoint = hitpoint;
        HitDirection = hitDirection;
        HitImpulse = hitImpulse;

        HitNormal = hitNormal;

        Source = source;
    }
}
