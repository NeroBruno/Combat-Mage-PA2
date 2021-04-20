using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public readonly Value<bool> Dragging = new Value<bool>();
    public readonly Value<bool> DraggingItem = new Value<bool>();
    public readonly Message PointerDown = new Message();
    public readonly Activity OnConsoleOpened = new Activity();


    //public Activity ItemWheel = new Activity();


    public Player Player { get; private set; }

    /// <summary>
    /// The main canvas used for GUI elements
    /// </summary>
    public Canvas Canvas { get { return _Canvas; } }


    public Font Font { get { return _Font; } }

    [SerializeField]
    private Canvas _Canvas = null;

    [SerializeField]
    private Font _Font = null;

    //[SerializeField]
    //private KeyCode _ItemWheelKey = KeyCode.Q;

    private UserInterfaceBehaviour[] _UIBehaviours;

    public void AttachToPlayer(Player player)
    {
        if (!_Canvas.isActiveAndEnabled)
            _Canvas.gameObject.SetActive(true);

        if (_UIBehaviours == null)
            _UIBehaviours = GetComponentsInChildren<UserInterfaceBehaviour>(true);

        Player = player;

        for (int i = 0; i < _UIBehaviours.Length; i++)
            _UIBehaviours[i].OnAttachment();

        for (int i = 0; i < _UIBehaviours.Length; i++)
            _UIBehaviours[i].OnPostAttachment();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            PointerDown.Send();

        //if (Input.GetKey(_ItemWheelKey))
        //{
        //    if (!ItemWheel.Active)
        //    {
        //        if (_ItemWheelKey.TryStart())
        //            Player.ViewLocked.Set(true);
        //    }
        //}
        //else if (_ItemWheelKey.Active && _ItemWheelKey.TryStop())
        //    Player.ViewLocked.Set(false);
    }
}
