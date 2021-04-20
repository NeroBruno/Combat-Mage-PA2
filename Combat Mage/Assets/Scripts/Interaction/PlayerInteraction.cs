using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : PlayerComponent
{
    [SerializeField]
    private Camera _WorldCamera = null;

    [SerializeField]
    [Tooltip("The maximum distance at which you can interact with objects")]
    private float _InteractionDistance = 2f;

    [SerializeField]
    [Range(0f, 60f)]
    private float _MaxInteractionAngle = 30f;

    [SerializeField]
    private LayerMask _LayerMask = new LayerMask();

    private InteractiveObject _InteractedObject;

    private void Awake()
    {
        Player.WantsToInteract.AddChangeListener(OnChanged_WantsToInteract);
    }

    private void OnChanged_WantsToInteract(bool wantsToInteract)
    {
        var rayCastData = Player.RaycastData.Get();

        var wantedToInteractPreviously = Player.WantsToInteract.GetPreviousValue();
        var wantsToInteractNow = wantsToInteract;

        if(rayCastData != null && rayCastData.IsInteractive)
        {
            if(!wantedToInteractPreviously && wantsToInteractNow)
            {
                rayCastData.InteractiveObject.OnInteractionStart(Player);
                _InteractedObject = rayCastData.InteractiveObject;
            }
        }

        if(_InteractedObject != null && wantedToInteractPreviously && !wantsToInteractNow)
        {
            _InteractedObject.OnInteractionEnd(Player);
            _InteractedObject = null;
        }
    }

    private void Update()
    {
        var ray = _WorldCamera.ViewportPointToRay(Vector2.one * 0.5f);
        var lastRaycastData = Player.RaycastData.Get();

        var collidersInRange = Physics.OverlapSphere(_WorldCamera.transform.position, _InteractionDistance, _LayerMask, QueryTriggerInteraction.Collide);
        float smallestAngle = 1000f;
        InteractiveObject closestObject = null;
        int closestObjectIndex = -1;

        Vector3 cameraPosition = _WorldCamera.transform.position;
        Vector3 cameraDiraction = _WorldCamera.transform.forward;

        for (int i = 0; i < collidersInRange.Length; i++)
        {
            InteractiveObject interactiveObject = collidersInRange[i].GetComponent<InteractiveObject>();

            RaycastHit hitInfo;

            if (interactiveObject != null && Physics.Linecast(cameraPosition, interactiveObject.transform.position + (interactiveObject.transform.position - cameraPosition).normalized * 0.05f, out hitInfo, _LayerMask))
            {
                if(hitInfo.collider == null || hitInfo.collider == collidersInRange[i])
                {
                    float angle = Vector3.Angle(cameraDiraction, interactiveObject.transform.position - cameraPosition);
                    if (angle < smallestAngle)
                    {
                        smallestAngle = angle;
                        closestObject = interactiveObject;
                        closestObjectIndex = i;
                    }
                }
            }
        }

        if (smallestAngle < _MaxInteractionAngle && ((lastRaycastData != null && lastRaycastData.Collider != collidersInRange[closestObjectIndex]) || lastRaycastData == null))
        {
            var raycastData = new RaycastData(collidersInRange[closestObjectIndex], closestObject);
            Player.RaycastData.Set(raycastData);

            bool startedRaycastingOnObject = lastRaycastData != null && raycastData.IsInteractive && raycastData.InteractiveObject != lastRaycastData.InteractiveObject;

            if (startedRaycastingOnObject)
                raycastData.InteractiveObject.OnRaycastStart(Player);
            else if (raycastData.IsInteractive)
                raycastData.InteractiveObject.OnRaycastUpdate(Player);
            else if (lastRaycastData != null && lastRaycastData.InteractiveObject != null)
                lastRaycastData.InteractiveObject.OnRaycastEnd(Player);
        }
        else if (smallestAngle > _MaxInteractionAngle)
        {
            Player.RaycastData.Set(null);

            // Let the object know the ray its not on it anymore
            if (lastRaycastData != null && lastRaycastData.IsInteractive)
            {
                if (lastRaycastData.IsInteractive)
                    lastRaycastData.InteractiveObject.OnRaycastEnd(Player);
            }
        }

        if (_InteractedObject != null)
            _InteractedObject.OnInteractionUpdate(Player);
    }
}
