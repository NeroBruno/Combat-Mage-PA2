using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceBehaviour : MonoBehaviour
{
    public UIManager Manager
    {
        get
        {
            if (!_Manager)
                _Manager = GetComponentInChildren<UIManager>();
            if (!_Manager)
                _Manager = GetComponentInParent<UIManager>();

            return _Manager;
        }
    }

    public Player Player { get { return Manager != null ? Manager.Player : null; } }

    //public Inventory PlayerStorage { get { return Player != null ? Player.Inventory : null; } }

    private UIManager _Manager;

    public virtual void OnAttachment() { }

    public virtual void OnPostAttachment() { }
}
