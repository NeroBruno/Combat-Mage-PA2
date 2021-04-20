using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastData
{
    public Collider Collider { get; private set; }

    public InteractiveObject InteractiveObject { get; private set; }

    public bool IsInteractive { get; private set; }

    public RaycastData(Collider collider, InteractiveObject interactiveObject)
    {
        Collider = collider;
        InteractiveObject = interactiveObject;
        IsInteractive = (InteractiveObject != null) && InteractiveObject.InteractionEnabled;
    }
}
