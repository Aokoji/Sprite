using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace customEvent
{
    /// <summary>
    /// S string,I int,F float,B bool,A Action,E enum,C custom
    /// </summary>
    public enum eventType
    {
        none,
        changePanel_S,
        panelChangeLoadingComplete,

        roundEnd_C,
        playRoundNext,
    }
    public class EventAction:CSingel<EventAction>
    {
        private Dictionary<eventType, Delegate> eventGather=new Dictionary<eventType, Delegate>();

        public delegate void act();
        public delegate void act<T>(T t);
        public delegate void act<T, U>(T t, U u);
        public delegate void act<T, U, V>(T t, U u, V v);
        public delegate void act<T, U, V, X>(T t, U u, V v, X x);
        public void AddEventGather(eventType arg,act action)
        {
            if (!eventGather.ContainsKey(arg))
                eventGather.Add(arg, null);
            eventGather[arg] = (act)eventGather[arg] + action;
        }
        public void AddEventGather<T>(eventType arg, act<T> action)
        {
            Debug.Log(typeof(T).ToString());
            if (!eventGather.ContainsKey(arg))
                eventGather.Add(arg, null);
            eventGather[arg] = (act<T>)eventGather[arg] + action;
        }
        public void AddEventGather<T, U>(eventType arg, act<T, U> action)
        {
            if (!eventGather.ContainsKey(arg))
                eventGather.Add(arg, null);
            eventGather[arg] = (act<T,U>)eventGather[arg] + action;
        }
        public void AddEventGather<T, U, V>(eventType arg, act<T, U, V> action)
        {
            if (!eventGather.ContainsKey(arg))
                eventGather.Add(arg, null);
            eventGather[arg] = (act<T, U, V>)eventGather[arg] + action;
        }
        public void AddEventGather<T, U, V, X>(eventType arg, act<T, U, V, X> action)
        {
            if (!eventGather.ContainsKey(arg))
                eventGather.Add(arg, null);
            eventGather[arg] = (act<T, U, V, X>)eventGather[arg] + action;
        }
        public void TriggerAction(eventType arg)
        {
            Debug.Log("triggerevent   " + arg.ToString());
            if (eventGather.ContainsKey(arg))
                ((act)eventGather[arg]).Invoke();
        }
        public void TriggerAction<T>(eventType arg,T t)
        {
            Debug.Log("triggerevent   " + arg.ToString());
            if (eventGather.ContainsKey(arg))
                ((act<T>)eventGather[arg])(t);
        }
        public void TriggerAction<T, U>(eventType arg,T t,U u)
        {
            Debug.Log("triggerevent   " + arg.ToString());
            if (eventGather.ContainsKey(arg))
                ((act<T, U>)eventGather[arg])(t, u);
        }
        public void TriggerAction<T, U, V>(eventType arg,T t,U u,V v)
        {
            Debug.Log("triggerevent   " + arg.ToString());
            if (eventGather.ContainsKey(arg))
                ((act<T, U, V>)eventGather[arg])(t, u, v);
        }
        public void TriggerAction<T, U, V, X>(eventType arg, T t, U u, V v,X x)
        {
            Debug.Log("triggerevent   " + arg.ToString());
            if (eventGather.ContainsKey(arg))
                ((act<T, U, V, X>)eventGather[arg])(t, u, v, x);
        }
        public void RemoveAction(eventType arg, act action)
        {
            if (eventGather.ContainsKey(arg))
                eventGather[arg] = (act)eventGather[arg] - action;
            if (eventGather[arg] == null)
                eventGather.Remove(arg);
        }
        public void RemoveAction<T>(eventType arg, act<T> action)
        {
            if (eventGather.ContainsKey(arg))
                eventGather[arg] = (act<T>)eventGather[arg] - action;
            if (eventGather[arg] == null)
                eventGather.Remove(arg);
        }
        public void RemoveAction<T, U>(eventType arg, act<T, U> action)
        {
            if (eventGather.ContainsKey(arg))
                eventGather[arg] = (act<T, U>)eventGather[arg] - action;
            if (eventGather[arg] == null)
                eventGather.Remove(arg);
        }
        public void RemoveAction<T, U, V>(eventType arg, act<T, U, V> action)
        {
            if (eventGather.ContainsKey(arg))
                eventGather[arg] = (act<T, U, V>)eventGather[arg] - action;
            if (eventGather[arg] == null)
                eventGather.Remove(arg);
        }
        public void RemoveAction<T, U, V, X>(eventType arg, act<T, U, V, X> action)
        {
            if (eventGather.ContainsKey(arg))
                eventGather[arg] = (act<T, U, V, X>)eventGather[arg] - action;
            if (eventGather[arg] == null)
                eventGather.Remove(arg);
        }

    }
}

