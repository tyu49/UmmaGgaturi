using System;
using System.Collections.Generic;
using UnityEngine;

namespace TYU.Script
{
    public static class IndividualEventBus<T>
    {
        private static Dictionary<T, Action> _eventDict = new();
        public static void Subscribe(T key, Action listener)
        {
            _eventDict.TryGetValue(key, out Action action);
            _eventDict[key] = action + listener;
        }
        public static void UnSubscribe(T key, Action listener)
        {
            if (!_eventDict.TryGetValue(key, out Action action))
                return;
            action -= listener;
            if(action == null)
                _eventDict.Remove(key);
            else
                _eventDict[key] = action;
        }
        public static void Publish(T key)
        {
            if (!_eventDict.ContainsKey(key)) return;
            _eventDict[key]?.Invoke();
        }
    }
}