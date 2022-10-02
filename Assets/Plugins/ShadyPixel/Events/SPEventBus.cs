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
            (bool keyExists, bool hasSubscription) = IsSubscribed(eventType, listener);

            if (!keyExists)
            {
                _lookup[eventType] = new List<ISPEventListenerBase>() { listener };
            }
            else if(!hasSubscription)
            {
                _lookup[eventType].Add(listener);
            }
        }
        public void RemoveListener<EventType>(ISPEventListenerBase<EventType> listener)
        {
            Type eventType = typeof(EventType);
            (_, bool hasSubscription) = IsSubscribed(eventType, listener);
            if (hasSubscription)
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
        private (bool, bool) IsSubscribed(Type type, ISPEventListenerBase receiver)
        {
            bool keyExists = false;
            bool subExists = false;

            if (_lookup.TryGetValue(type, out List<ISPEventListenerBase> subscriptions))
            {
                keyExists = true;
                subExists = subscriptions.IndexOf(receiver) >= 0;
            }

            return (keyExists, subExists);
        }
    }
}