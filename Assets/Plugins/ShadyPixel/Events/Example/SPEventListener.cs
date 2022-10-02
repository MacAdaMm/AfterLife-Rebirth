using ShadyPixel.Events;
using System;

namespace ShadyPixel.Events
{
    public class SPEventListener<T> : ISPEventListenerBase<T> where T : SPEventBase
    {
        public Action<T> OnCallback { get; private set; }

        private Action<T> _callback;
        private SPEventBus _eventBus;

        public SPEventListener(SPEventBus eventBus, Action<T> callback, bool startListening= true)
        {
            _eventBus = eventBus;
            _callback = callback;

            OnCallback += _callback;

            if (startListening)
            {
                _eventBus.AddListener(this);
            }
        }

        public void StartListening()
        {
            _eventBus.AddListener(this);
        }
        public void StopListening()
        {
            _eventBus.RemoveListener(this);
        }
        public void OnEvent(T eventType)
        {
            OnCallback?.Invoke(eventType);
        }
    }
}

