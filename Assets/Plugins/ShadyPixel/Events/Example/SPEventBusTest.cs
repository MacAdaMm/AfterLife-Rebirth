using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Events;

public partial class SPEventBusTest : MonoBehaviour, ISPEventListenerBase<SPGameEvent>
{
    /* IMPORTENT NOTE: If creating a static EventBus you have to RemoveListeners or the listeners will create a MemoryLeak because of the reference held in the EventBus!! */

    // NOTE: The EventBus would idealy live on some sort of mediator object that everything would use to Listen/Emit to events.
    // Here we'll just create one on here for testing purposes.
    public SPEventBus _eventBus { get; } = new SPEventBus();

    // If using a SPEventlistener we need a field to hold the reference to it.
    SPEventListener<SPGameEvent> _gameEventListener;

    // Our example callback used with interface ISPListenerBase implementation.
    public void OnEvent(SPGameEvent eventType)
    {
        Debug.Log($"{typeof(SPGameEvent).Name} Triggered. Callback ran by GameObject: {gameObject.name} -> Component: {this.GetType().Name}");
    }

    //Our example callback used with the SPEventListener
    private void OnSPGameEvent(SPGameEvent gameEvent)
    {
        Debug.Log($"{typeof(SPGameEvent).Name} Triggered from WrappedEventListener. Callback ran by GameObject: {gameObject.name} -> Component: {this.GetType().Name}");
    }

    // Here We hook everything up.
    void Start()
    {   // Using SPEventListener requires a reference to the Event bus and the callback we want to respond with when a event is emited.
        // this also optionaly Starts listening for the events when created. startListening defaults to true.
        _gameEventListener = new SPEventListener<SPGameEvent>(_eventBus, OnSPGameEvent, startListening: false);

        /*
         * If using SPEventListener you have three options to start listening for events.
         * 
         * 1. When instanciating the SPEventListener you can start listening by having startListening = true, or by just omiting it altogeather (its true by default)
         * _gameEventListener = new SPEventListener<SPGameEvent>(_eventBus, OnSPGameEvent, true);
         * _gameEventListener = new SPEventListener<SPGameEvent>(_eventBus, OnSPGameEvent);
         * 
         * 2. Call the .StartListening method on the SPEventListener object.
         * _gameEventListener.StartListening();
         * 
         * 3. Call the AddListener method on the EventBus with the SPEventListener as the parameter.
         * _eventBus.Add(_gameEventListener);
         */
        _gameEventListener.StartListening();
        // If implementing the ISPEventListener directly. we need to add the listener to the EventBus to start listening for events.
        // If more than one interface is used on a class them you must add the generic type <EventType> to the method call the EventType to Add/Remove Listener.
        _eventBus.AddListener(this);

        // Emiting events is always done with the EventBus.
        _eventBus.EmitEvent(new SPGameEvent("Test Event"));
    }

    private void OnDisable()
    {
        /* IMPORTENT NOTE: If creating a static EventBus you have to RemoveListeners or the listeners will create a MemoryLeak because of the reference held in the EventBus!! */

        // If implementing the ISPEventListener directly, use RemoveListener to stop listening for events.
        _eventBus.RemoveListener(this);

        // If using SPEventListener You can Use Either RemoveListener on the EventBus or StopListening on the SPEventListener.
        _gameEventListener.StopListening();
        //_eventBus.RemoveListener(_gameEventListener);
    }
}
