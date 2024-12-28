using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public static class EventHandler
{
    public delegate void Action<T>(T arg);

    public delegate void Action<T1, T2>(T1 arg1, T2 arg2);

    private static Dictionary<BeUnit, Dictionary<string, Delegate>> events;

    public static void Init()
    {
        events = new Dictionary<BeUnit, Dictionary<string, Delegate>>();
    }

    public static void UnInit()
    {
        if (events != null && events.Count > 0)
        {
            foreach (var temp in events)
            {
                temp.Value.Clear();
            }

            events.Clear();
        }

        events = null;
    }

    public static void RegisterEvent(BeUnit beUnit, string eventName, Action action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }

    public static void RegisterEvent<T>(BeUnit beUnit, string eventName, Action<T> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }

    public static void RegisterEvent<T1, T2>(BeUnit beUnit, string eventName, Action<T1, T2> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }

    public static void RegisterEvent<T1, T2, T3>(BeUnit beUnit, string eventName, Action<T1, T2, T3> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }

    public static void RegisterEvent<T1, T2, T3, T4>(BeUnit beUnit, string eventName, Action<T1, T2, T3, T4> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }

    public static void RegisterEvent<T1, T2, T3, T4, T5>(BeUnit beUnit, string eventName,
        Action<T1, T2, T3, T4, T5> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }

    public static void RegisterEvent<T1, T2, T3, T4, T5, T6>(BeUnit beUnit, string eventName,
        Action<T1, T2, T3, T4, T5, T6> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for RegisterEvent.");
            return;
        }

        if (!events.TryGetValue(beUnit, out var unitEvents))
        {
            unitEvents = new Dictionary<string, Delegate>();
            events[beUnit] = unitEvents;
        }

        if (unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            unitEvents[eventName] = Delegate.Combine(existingDelegate, action);
        }
        else
        {
            unitEvents[eventName] = action;
        }
    }


    public static void UnRegisterEvent(BeUnit beUnit, string eventName, Action action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }

    public static void UnRegisterEvent<T>(BeUnit beUnit, string eventName, Action<T> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }

    public static void UnRegisterEvent<T1, T2>(BeUnit beUnit, string eventName, Action<T1, T2> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }

    public static void UnRegisterEvent<T1, T2, T3>(BeUnit beUnit, string eventName, Action<T1, T2, T3> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }

    public static void UnRegisterEvent<T1, T2, T3, T4>(BeUnit beUnit, string eventName, Action<T1, T2, T3, T4> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }

    public static void UnRegisterEvent<T1, T2, T3, T4, T5>(BeUnit beUnit, string eventName,
        Action<T1, T2, T3, T4, T5> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }

    public static void UnRegisterEvent<T1, T2, T3, T4, T5, T6>(BeUnit beUnit, string eventName,
        Action<T1, T2, T3, T4, T5, T6> action)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName) || action == null)
        {
            Debug.LogError("Invalid parameters for UnRegisterEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            var newDelegate = Delegate.Remove(existingDelegate, action);

            if (newDelegate == null)
            {
                unitEvents.Remove(eventName);

                if (unitEvents.Count == 0)
                    events.Remove(beUnit);
            }
            else
            {
                unitEvents[eventName] = newDelegate;
            }
        }
    }


    public static void ExecuteEvent(BeUnit beUnit, string eventName)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action action)
            {
                action.Invoke();
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }

    public static void ExecuteEvent<T>(BeUnit beUnit, string eventName, T arg)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action<T> action)
            {
                action.Invoke(arg);
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }

    public static void ExecuteEvent<T1, T2>(BeUnit beUnit, string eventName, T1 arg1, T2 arg2)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action<T1, T2> action)
            {
                action.Invoke(arg1, arg2);
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }

    public static void ExecuteEvent<T1, T2, T3>(BeUnit beUnit, string eventName, T1 arg1, T2 arg2, T3 arg3)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action<T1, T2, T3> action)
            {
                action.Invoke(arg1, arg2, arg3);
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }

    public static void ExecuteEvent<T1, T2, T3, T4>(BeUnit beUnit, string eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action<T1, T2, T3, T4> action)
            {
                action.Invoke(arg1, arg2, arg3, arg4);
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }

    public static void ExecuteEvent<T1, T2, T3, T4, T5>(BeUnit beUnit, string eventName, T1 arg1, T2 arg2, T3 arg3,
        T4 arg4, T5 arg5)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action<T1, T2, T3, T4, T5> action)
            {
                action.Invoke(arg1, arg2, arg3, arg4, arg5);
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }

    public static void ExecuteEvent<T1, T2, T3, T4, T5, T6>(BeUnit beUnit, string eventName, T1 arg1, T2 arg2, T3 arg3,
        T4 arg4, T5 arg5, T6 arg6)
    {
        if (beUnit == null || string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Invalid parameters for ExecuteEvent.");
            return;
        }

        if (events.TryGetValue(beUnit, out var unitEvents) &&
            unitEvents.TryGetValue(eventName, out var existingDelegate))
        {
            if (existingDelegate is Action<T1, T2, T3, T4, T5, T6> action)
            {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }
            else
            {
                Debug.LogError($"Event '{eventName}' parameter type mismatch for {beUnit.name}.");
            }
        }
        else
        {
            Debug.LogError($"Event '{eventName}' not found for {beUnit.name}.");
        }
    }
}