using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Events;

public class SPEventBusTest : MonoBehaviour, ISPEventListenerBase<SPGameEvent>
{
    private SPEventBus _eventBus = new SPEventBus();

    public void OnEvent(SPGameEvent eventType)
    {
        Debug.Log($"{typeof(SPGameEvent).Name} Triggered. Callback ran by GameObject: {gameObject.name} -> Component: {this.GetType().Name}");
    }

    // Start is called before the first frame update
    void Start()
    {
        _eventBus.AddListener(this);

        _eventBus.EmitEvent(new SPGameEvent("Test Event"));
    }

    private void OnDisable()
    {
        _eventBus.RemoveListener(this);
    }
}
