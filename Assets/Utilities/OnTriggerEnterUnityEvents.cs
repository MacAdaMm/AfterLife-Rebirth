using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterUnityEvents : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _unityEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _unityEvent.Invoke();
    }
}
