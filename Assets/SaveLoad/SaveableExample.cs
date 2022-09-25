using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableExample : MonoBehaviour, ISaveable
{
    private bool _isEnabled;

    private void OnEnable()
    {
        _isEnabled = true;
    }

    private void OnDisable()
    {
        _isEnabled = false;
    }

    public object CaptureState()
    {
        return _isEnabled;
    }

    public void RestoreState(object state)
    {
        _isEnabled = (bool)state;
        enabled = _isEnabled;
    }
}
