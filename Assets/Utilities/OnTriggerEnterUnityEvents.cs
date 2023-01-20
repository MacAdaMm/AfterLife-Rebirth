using ShadyPixel.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterUnityEvents : MonoBehaviour, ISaveable
{
    [SerializeField]
    private UnityEvent _unityEvent;

    public object CaptureState()
    {
        return enabled;
    }

    public void RestoreState(object state)
    {
        enabled = (bool)state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled)
        {
            _unityEvent.Invoke();
        }
    }
}
