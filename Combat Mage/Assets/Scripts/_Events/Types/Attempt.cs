using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool TryerDelegate();

/// <summary>
/// 
/// </summary>
public class Attempt
{
    public float LastExecutionTime { get { return _LastExecutionTime; } private set { } }

    private TryerDelegate _Tryer;
    private Action _Listeners;

    private float _LastExecutionTime;

    /// <summary>
    /// Registers a method that will try to execute this action
    /// Only 1 tryer is allowed
    /// </summary>
    /// <param name="tryer"></param>
    public void SetTryer(TryerDelegate tryer)
    {
        _Tryer = tryer;
    }

    public void AddListener(Action listener)
    {
        _Listeners += listener;
    }

    public void RemoveListener(Action listener)
    {
        _Listeners -= listener;
    }

    public bool Try()
    {
        bool wasSuccessful = (_Tryer == null || _Tryer());
        if (wasSuccessful)
        {
            if (_Listeners != null)
            {
                _Listeners();

                _LastExecutionTime = Time.time;
            }
            return true;
        }

        return false;
    }
}

public class Attempt<T>
{
    public delegate bool GenericTryerDelegate(T arg);

    private GenericTryerDelegate _Tryer;
    private Action<T> _Listeners;

    /// <summary>
    /// Registers a method that will try to execute this action
    /// Only 1 tryer is allowed
    /// </summary>
    /// <param name="tryer"></param>
    public void SetTryer(GenericTryerDelegate tryer)
    {
        _Tryer = tryer;
    }

    public void AddListener(Action<T> listener)
    {
        _Listeners += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        _Listeners -= listener;
    }

    public bool Try(T arg)
    {
        bool wasSuccessful = (_Tryer == null || _Tryer(arg));
        if (wasSuccessful)
        {
            if (_Listeners != null)
                _Listeners(arg);

            return true;
        }

        return false;
    }
}

public class Attempt<T, V>
{
    public delegate bool GenericTryerDelegate(T arg1, V arg2);

    private GenericTryerDelegate _Tryer;
    private Action<T, V> _Listeners;

    /// <summary>
    /// Registers a method that will try to execute this action
    /// Only 1 tryer is allowed
    /// </summary>
    /// <param name="tryer"></param>
    public void SetTryer(GenericTryerDelegate tryer)
    {
        _Tryer = tryer;
    }

    public void AddListener(Action<T, V> listener)
    {
        _Listeners += listener;
    }

    public void RemoveListener(Action<T, V> listener)
    {
        _Listeners -= listener;
    }

    public bool Try(T arg1, V arg2)
    {
        bool wasSuccessful = (_Tryer == null || _Tryer(arg1, arg2));
        if (wasSuccessful)
        {
            if (_Listeners != null)
                _Listeners(arg1, arg2);

            return true;
        }

        return false;
    }
}


