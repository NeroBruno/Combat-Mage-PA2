using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the base class for any entity
/// </summary>
public class LivingEntity : MonoBehaviour
{
    public AudioSource AudioSource { get => _AudioSource; }

    public Animator Animator { get => _Animator; }

    public readonly Value<float> Health = new Value<float>(100f);

    public readonly Attempt<HealthEventData> ChangeHealth = new Attempt<HealthEventData>();

    public readonly Value<bool> IsGrounded = new Value<bool>(true);

    public readonly Value<Vector3> Velocity = new Value<Vector3>(Vector3.zero);

    [HideInInspector]
    public Value<Vector3> LookDirection = new Value<Vector3>();

    public readonly Message<float> FallImpact = new Message<float>();

    public readonly Message Death = new Message();

    public readonly Message Respawn = new Message();

    public Hitbox[] Hitboxes;

    /// <summary>
    /// Spells that are currently bound to an effect, such as Attack, Defense or Utility/Support and its element / (or elements)
    /// </summary>
    //public Spells BoundSpells { get { return _BoundSpells; } }

    [Header("Main Components")]

    [SerializeField]
    private AudioSource _AudioSource = null;

    [SerializeField]
    private Animator _Animator = null;

    //[SerializeField]
    //private Spells _BoundSpells = null;

    private void Start()
    {
        Hitboxes = GetComponentsInChildren<Hitbox>();

        foreach (var component in GetComponentsInChildren<LivingEntityComponent>(true))
            component.OnEntityStart();
    }
}
