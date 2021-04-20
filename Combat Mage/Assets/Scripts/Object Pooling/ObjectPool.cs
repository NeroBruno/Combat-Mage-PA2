using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public string Id { get => _Id; }

    private GameObject _Template;
    private Transform _Parent;
    private string _Id;

    private List<PoolableObject> _AvailableObjects;
    private List<PoolableObject> _InUseObjects;

    private int _MinSize;
    private int _MaxSize;
    private int _CurrentSize;

    private bool _Initialized;

    private float _LastObjectGetTime;

    private bool _AutoShrink;
    private float _NextShrinkTime;
    private float _AutoReleaseDelay;

    public ObjectPool(string id, GameObject template, int minSize, int maxSize, bool autoShrink, float autoReleaseDelay, Transform parent)
    {
        if (template == null)
        {
            Debug.LogError("You want to create an object pool for an object that is null...");
            return;
        }

        // Create and store a new template and its hash code
        _Template = Object.Instantiate(template, parent);
        _Template.SetActive(false);
        _Parent = parent;
        _Id = id;

        // Store the min and max sizes for this pool
        _MinSize = minSize;
        _MaxSize = Mathf.Clamp(maxSize, _MinSize, int.MaxValue);
        _CurrentSize = _MinSize;

        // Initialize the lists
        _AvailableObjects = new List<PoolableObject>(_MaxSize);
        _InUseObjects = new List<PoolableObject>(_MaxSize);

        // Spawn the default objects (more will be spawned if needed according to minSize and maxSize)
        for (int i = 0; i < _CurrentSize; i++)
        {
            PoolableObject obj = CreateNewObject(_Template, _Parent, _Id);
            obj.gameObject.SetActive(false);
            _AvailableObjects.Add(obj);
        }

        _AutoShrink = autoShrink;
        _AutoReleaseDelay = autoReleaseDelay;

        // Mark this pool as initialized, means it can now be used
        _Initialized = true;
    }

    public void Update()
    {
        if (_AutoShrink && _AvailableObjects.Count > _MinSize && Time.time > _LastObjectGetTime + 60f && Time.time > _NextShrinkTime)
        {
            PoolableObject objToDestroy = _AvailableObjects[_AvailableObjects.Count - 1];
            _AvailableObjects.RemoveAt(_AvailableObjects.Count - 1);

            Object.Destroy(objToDestroy);

            _NextShrinkTime = Time.time + 0.5f;
            _CurrentSize--;
        }
    }

    public PoolableObject GetObject()
    {
        if (_Initialized)
        {
            Debug.LogError("This pool can not be used, its not initialized properly");
            return null;
        }

        PoolableObject obj = null;

        if (_AvailableObjects.Count > 0)
        {
            obj = _AvailableObjects[_AvailableObjects.Count - 1];

            // Move the object into the 'in use list'
            _AvailableObjects.RemoveAt(_AvailableObjects.Count - 1);
            _InUseObjects.Add(obj);
        }
        // Grow the pool
        else if (_CurrentSize < _MaxSize)
        {
            _CurrentSize++;
            obj = CreateNewObject(_Template, _Parent, _Id);
            _InUseObjects.Add(obj);
        }
        // The pool has reached its max size, use the oldest in use object
        else
        {
            obj = _InUseObjects[0];

            _InUseObjects[0] = _InUseObjects[_InUseObjects.Count - 1];
            _InUseObjects[_InUseObjects.Count - 1] = obj;
        }

        _LastObjectGetTime = Time.time;
        obj.gameObject.SetActive(true);
        obj.OnUse();

        if (_AutoReleaseDelay != Mathf.Infinity)
            PoolingManager.Instance.QueueObjectRelease(obj, _AutoReleaseDelay);

        return obj;
    }

    public bool TryPoolObject(PoolableObject obj)
    {
        if (!_Initialized)
        {
            Debug.LogError("This pool can not be used, it's not initialized properly!");
            return false;
        }

        if (obj == null)
        {
            Debug.LogError("The object you want to pool is null!!");
            return false;
        }

        if (_Id != obj.PoolId)
        {
            Debug.LogError("You want to put an object back in this pool, but it doesn't belong here!!");
            return false;
        }

        _InUseObjects.Remove(obj);
        _AvailableObjects.Add(obj);

        obj.OnReleased();
        obj.transform.SetParent(_Parent);
        obj.gameObject.SetActive(false);

        return true;
    }

    private PoolableObject CreateNewObject(GameObject template, Transform parent, string poolId)
    {
        if (template == null)
            return null;

        GameObject obj = Object.Instantiate(template, parent);
        PoolableObject poolableObj = obj.GetComponent<PoolableObject>();

        if (poolableObj == null)
            poolableObj = obj.AddComponent<PoolableObject>();

        poolableObj.Init(poolId);

        return poolableObj;
    }
}
