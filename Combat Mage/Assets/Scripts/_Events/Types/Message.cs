using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public float LastCallTime { get { return _CallTime; } private set { } }

    private Action _Callbacks;

    private float _CallTime;

    public void AddListener(Action callback)
    {
        _Callbacks += callback;
    }

    public void RemoveListener(Action callback)
    {
        _Callbacks -= callback;
    }

    public void Send()
    {
        if (_Callbacks != null)
        {
            _CallTime = Time.time;

            _Callbacks();
        }
    }
}

public class Message<T>
{
    private Action<T> _Callbacks;

    public void AddListener(Action<T> callback)
    {
        _Callbacks += callback;
    }

    public void RemoveListener(Action<T> callback)
    {
        _Callbacks -= callback;
    }

    public void Send(T arg)
    {
        if (_Callbacks != null)
            _Callbacks(arg);
    }
}

public class Message<T, V>
{
    private Action<T, V> _Callbacks;

    public void AddListener(Action<T, V> callback)
    {
        _Callbacks += callback;
    }

    public void RemoveListener(Action<T, V> callback)
    {
        _Callbacks -= callback;
    }

    public void Send(T arg1, V arg2)
    {
        if (_Callbacks != null)
            _Callbacks(arg1, arg2);
    }
}

public class Message<T, V, K>
{
    private Action<T, V, K> _Callbacks;

    public void AddListener(Action<T, V, K> callback)
    {
        _Callbacks += callback;
    }

    public void RemoveListener(Action<T, V, K> callback)
    {
        _Callbacks -= callback;
    }

    public void Send(T arg1, V arg2, K arg3)
    {
        if (_Callbacks != null)
            _Callbacks(arg1, arg2, arg3);
    }
}