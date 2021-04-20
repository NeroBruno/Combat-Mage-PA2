using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<string, ObjectPool> _Pools = new Dictionary<string, ObjectPool>(50);
    private SortedList<float, PoolableObject> _ObjectsToRelease = new SortedList<float, PoolableObject>();

    public ObjectPool CreatePool(GameObject template, int minSize, int maxSize, bool autoShrink, string poolId, float autoReleaseDelay = Mathf.Infinity)
    {
        ObjectPool pool;

        if (!_Pools.TryGetValue(poolId, out pool))
        {
            pool = new ObjectPool(poolId, template, minSize, maxSize, autoShrink, autoReleaseDelay, transform);

            _Pools.Add(poolId, pool);
        }

        return pool;
    }

    /// <summary>
    /// This method will use the prefab's instance id as a poolId to create a pool if one doesn't exist (it's the closest thing in ease of use to Object.Instantiate())<br></br>
    /// You can also use CreatePool() to create a custom pool for your prefabs.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public PoolableObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        PoolableObject obj = null;

        if (prefab != null)
        {
            ObjectPool pool;

            if (_Pools.TryGetValue(prefab.GetInstanceID().ToString(), out pool))
            {
                obj = pool.GetObject();
            }
            else
            {
                pool = CreatePool(prefab, 10, 30, true, prefab.GetInstanceID().ToString());
                obj = pool.GetObject();
            }
        }

        if (obj != null)
        {
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.SetParent(parent);
        }

        return obj;
    }

    /// <summary>
    /// This method will use the prefab's instance id as a poolId to create a pool if one doesn't exist (it's the closest thing in ease of use to Object.Instantiate())<br></br>
    /// You can also use CreatePool() to create a custom pool for your prefabs.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public PoolableObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        PoolableObject obj = null;

        if (prefab != null)
        {
            ObjectPool pool;

            if (_Pools.TryGetValue(prefab.GetInstanceID().ToString(), out pool))
            {
                obj = pool.GetObject();
            }
            else
            {
                pool = CreatePool(prefab, 10, 30, true, prefab.GetInstanceID().ToString());
                obj = pool.GetObject();
            }
        }

        if (obj != null)
            obj.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    public PoolableObject GetObject(string poolId, Vector3 position, Quaternion rotation, Transform parent)
    {
        PoolableObject obj = GetObject(poolId);

        if (obj != null)
        {
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.SetParent(parent);
        }

        return obj;
    }

    public PoolableObject GetObject(string poolId, Vector3 position, Quaternion rotation)
    {
        PoolableObject obj = GetObject(poolId);

        if (obj != null)
            obj.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    public PoolableObject GetObject(string poolId)
    {
        ObjectPool pool = null;
        _Pools.TryGetValue(poolId, out pool);

        if (pool != null)
            return pool.GetObject();
        else
            return null;
    }

    public bool ReleaseObject(PoolableObject obj)
    {
        if (obj == null)
            return false;

        ObjectPool pool = null;

        if (!_Pools.ContainsKey(obj.PoolId))
            print("key not found: " + obj.PoolId);

        _Pools.TryGetValue(obj.PoolId, out pool);

        if (pool != null)
            return pool.TryPoolObject(obj);
        else
            return false;
    }

    public void QueueObjectRelease(PoolableObject obj, float delay)
    {
        float key = Time.time + delay;

        if (_ObjectsToRelease.Count > 0 && Time.time > _ObjectsToRelease.Keys[0])
        {
            ReleaseObject(_ObjectsToRelease.Values[0]);
            _ObjectsToRelease.RemoveAt(0);
        }
    }
}
