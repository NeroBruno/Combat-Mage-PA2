using System;
using UnityEngine;

public class Activity
{
    public bool Active { get { return _Active; } private set { } }
    public float StartTime { get { return _StartTime; } private set { } }
    public float EndTime { get { return _EndTime; } private set { } }

    private TryerDelegate _StartTryers;
    private TryerDelegate _StopTryers;
    private Action _OnStart;
    private Action _OnStop;

    private bool _Active;
    private float _StartTime;
    private float _EndTime;

    /// <summary>
    /// This will register a method that will approve or disapprove the starting of this activity
    /// </summary>
    /// <param name="tryer"></param>
    public void AddStartTryer(TryerDelegate tryer)
    {
        _StartTryers = tryer;
    }

    /// <summary>
    /// This will register a method that will approve or disapprove the stopping of this activity
    /// </summary>
    /// <param name="tryer"></param>
    public void AddStopTryer(TryerDelegate tryer)
    {
        _StopTryers = tryer;
    }

    /// <summary>
    /// Will be called when this activity starts
    /// </summary>
    /// <param name="listener"></param>
    public void AddStartListener(Action listener)
    {
        _OnStart += listener;
    }

    /// <summary>
    /// Will be called when this activity stops
    /// </summary>
    /// <param name="listener"></param>
    public void AddStopListener(Action listener)
    {
        _OnStop += listener;
    }

    public void RemoveStartListener(Action listener)
    {
        _OnStart -= listener;
    }

    public void RemoveStopListener(Action listener)
    {
        _OnStop -= listener;
    }

    public void ForceStart()
    {
        if (_Active)
            return;

        _Active = true;
        _StartTime = Time.time;

        if (_OnStart != null)
            _OnStart();
    }

    public bool TryStart()
    {
        if (_Active)
            return false;

        if (_StartTryers != null)
        {
            bool activityStarted = CallStartApprovers();

            if (activityStarted)
            {
                _Active = true;
                _StartTime = Time.time;
            }

            if (activityStarted && _OnStart != null)
                _OnStart();

            return activityStarted;
        }
        else
            Debug.LogWarning("[Activity] - You tried to start an activity which has no tryer (if no one checks if the activity can start, it won't start)");

        return false;
    }

    public bool TryStop()
    {
        if (!_Active)
            return false;

        if(_StopTryers != null)
        {
            if (CallStopApprovers())
            {
                _Active = false;
                _EndTime = Time.time;

                if (_OnStop != null)
                    _OnStop();

                return true;
            }
        }

        return false;
    }

    public void ForceStop()
    {
        if (!_Active)
            return;

        _Active = false;
        _EndTime = Time.time;

        if (_OnStop != null)
            _OnStop();
    }

    private bool CallStartApprovers()
    {
        var invocationList = _StartTryers.GetInvocationList();

        for (int i = 0; i < invocationList.Length; i++)
        {
            if (!(bool)invocationList[i].DynamicInvoke())
                return false;
        }

        return true;
    }

    private bool CallStopApprovers()
    {
        var invocationList = _StopTryers.GetInvocationList();

        for (int i = 0; i < invocationList.Length; i++)
        {
            if (!(bool)invocationList[i].DynamicInvoke())
                return false;
        }

        return true;
    }
}
