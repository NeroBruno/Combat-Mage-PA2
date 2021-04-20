using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for interactive objects (buttons, pickups)
/// Has numerous raycast and interaction callbacks which are overridable
/// </summary>
public class InteractiveObject : MonoBehaviour
{
    public bool InteractionEnabled { get { return _InteractionEnabled; } set { _InteractionEnabled = value; } }
    public string InteractionText { get { return _InteractionText; } }

    [Header("Interaction")]

    [SerializeField]
    private bool _InteractionEnabled = true;

    [SerializeField]
    [Multiline]
    protected string _InteractionText = string.Empty;

    //[SerializeField]
    //private SoundPlayer _RaycastStartAudio = null;

    //[SerializeField]
    //private SoundPlayer _RaycastEndAudio = null;

    //[SerializeField]
    //private SoundPlayer _InteractionStartAudio = null;

    //[SerializeField]
    //private SoundPlayer _InteractionEndAudio = null;

    public UnityEvent _InteractEvent = null;

    /// <summary>
    /// Called when a player starts looking at the object (in range)
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnRaycastStart(Player player)
    {
        //_RaycastStartAudio.Play2D();
    }

    /// <summary>
    /// Called while a player is looking at the object
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnRaycastUpdate(Player player) { }

    /// <summary>
    /// Called when a player stops looking at the object (in range)
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnRaycastEnd(Player player)
    {
        //_RaycastEndAudio.Play2D();
    }
    
    /// <summary>
    /// Called when a player starts interacting with the object
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnInteractionStart(Player player)
    {
        //_InteractionStartAudio.Play2D();
        _InteractEvent.Invoke();
    }

    /// <summary>
    /// Called while a player is interacting with the object
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnInteractionUpdate(Player player) { }

    /// <summary>
    /// Called when a player stops interacting with the object
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnInteractionEnd(Player player)
    {
        //_InteractionEndAudio.Play2D();
    }
}
