using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to have a callback when the value changes such as (for example, updating the GUI when the player takes damage)
/// </summary>
[Serializable]
public class Value<T>
{
    public delegate T Filter(T previousValue, T newValue);

    public T Val { get { return _CurrentValue; } }

    public T PreVal { get { return _PreviousValue; } }

    [NonSerialized]
    private Action<T> _Set;

    [NonSerialized]
    private Filter _Filter;

    [NonSerialized]
    private T _CurrentValue;

    [NonSerialized]
    private T _PreviousValue;

    public Value()
    {
        _CurrentValue = default(T);
        _PreviousValue = default(T);
    }

    public Value(T initialValue)
    {
        _CurrentValue = initialValue;
        _PreviousValue = _CurrentValue;
    }

    internal void AddChangeListener()
    {
        throw new NotImplementedException();
    }

    public void AddChangeListener(Action<T> callback)
    {
        _Set += callback;
    }

    public void RemoveChangeListner(Action<T> callback)
    {
        _Set -= callback;
    }

    /// Filter function called before the regular callbacks, used for clamping values like player health, etc
    public void SetFilter(Filter filter)
    {
        _Filter = filter;
    }

    /// <summary>
    /// Returns current value
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        return _CurrentValue;
    }

    /// <summary>
    /// Returns previous value
    /// </summary>
    /// <returns></returns>
    public T GetPreviousValue()
    {
        return _PreviousValue;
    }

    public void Set(T value)
    {
        _PreviousValue = _CurrentValue;
        _CurrentValue = value;

        if(_Filter != null)
            _CurrentValue = _Filter(_PreviousValue, _CurrentValue);

        if(_Set != null && ((_PreviousValue == null && _CurrentValue != null) || (_PreviousValue != null && !_PreviousValue.Equals(_CurrentValue))))
            _Set(_CurrentValue);
    }

    public void SetAndForceUpdate(T value)
    {
        _PreviousValue = _CurrentValue;
        _CurrentValue = value;

        if(_Filter != null)
            _CurrentValue = _Filter(_PreviousValue, _CurrentValue);

        if(_Set != null)
            _Set(_CurrentValue);
    }

    public void SetAndDontUpdate(T value)
    {
        _PreviousValue = _CurrentValue;
        _CurrentValue = value;

        if (_Filter != null)
            _CurrentValue = _Filter(_PreviousValue, _CurrentValue);
    }

    public bool Is(T value)
    {
        return _CurrentValue != null && _CurrentValue.Equals(value);
    }
}
