using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Enemy class handler for states and activities
/// </summary>
public class Enemy : LivingEntity
{
    // We should put sensors and raycasts view of the world here
    // Use this class as a way to know what the entity is doing and its state

    public Value<bool> IsStunned = new Value<bool>();

    public Value<bool> IsRagdoll = new Value<bool>();

    public Activity Chase = new Activity();

    public Activity Patrol = new Activity();

    //public Value<float> MovementSpeedFactor = new Value<float>(1f);

    public Attempt Attack = new Attempt();

    public Attempt Dodge = new Attempt();

}
