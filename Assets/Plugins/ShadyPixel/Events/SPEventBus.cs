using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadyPixel.Events
{
    public class SPEventBus 
    {
        private Dictionary<Type, List<ISPEventListenerBase>> _lookup;

        public SPEventBus()
        {
            _lookup = new Dictionary<Type, List<ISPEventListenerBase>>();
        }
        public void AddListener<EventType>(ISPEventListenerBase<EventType> listener)
        {
            Type eventType = typeof(EventType);

            if (_lookup.ContainsKey(eventType))
            {
                _lookup[eventType].Add(listener);
            }
            else
            {
                _lookup[eventType] = new List<ISPEventListenerBase>() { listener };
            }
        }
        public void RemoveListener<EventType>(ISPEventListenerBase<EventType> listener)
        {
            Type eventType = typeof(EventType);

            if (IsSubscribed(eventType, listener))
            {
                _lookup[eventType].Remove(listener);
            }
        }
        public void EmitEvent<EventType>(EventType newEvent)
        {
            Type eventType = newEvent.GetType();

            if (_lookup.TryGetValue(eventType, out List<ISPEventListenerBase> subscriptions))
            {
                for (int i = 0; i < subscriptions.Count; i++)
                {
                    (subscriptions[i] as ISPEventListenerBase<EventType>).OnEvent(newEvent);
                }
            }
        }
        private bool IsSubscribed(Type type, ISPEventListenerBase receiver)
        {
            bool exists = false;

            if (_lookup.TryGetValue(type, out List<ISPEventListenerBase> subscriptions))
            {
                exists = subscriptions.IndexOf(receiver) > 0;
            }

            return exists;
        }
    }
}