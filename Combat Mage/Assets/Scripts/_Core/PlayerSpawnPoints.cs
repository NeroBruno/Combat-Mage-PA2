using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoints : MonoBehaviour
{
    [SerializeField]
    private float _MaxVerticalRaycast = 2f;

    [SerializeField]
    private LayerMask _GroundLayerMask = 0;

    private Transform[] _SpawnPoints;

    public Vector3 GetRandomSpawnPoint()
    {
        if (_SpawnPoints != null)
        {
            Vector3 spawnPoint = _SpawnPoints[Random.Range(0, _SpawnPoints.Length)].position;

            RaycastHit hit;

            if (Physics.Raycast(spawnPoint, -transform.up, out hit, _MaxVerticalRaycast, _GroundLayerMask))
            {
                spawnPoint = hit.point + (Vector3.up * 0.1f);
            }

            return spawnPoint;
        }
        else return Vector3.zero;
    }

    public Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    private void Awake()
    {
        _SpawnPoints = GetComponentsInChildren<Transform>();
    }
}
