using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lockable : MonoBehaviour
{
    [field: SerializeField]
    public bool IsLocked { get; protected set; }

    [field: SerializeField]
    public UnityEvent OnLock { get; private set; }

    [field: SerializeField]
    public UnityEvent OnUnlock { get; private set; }

    private Func<bool> _conditions;

    public void Lock()
    {
        if (!IsLocked)
        {
            IsLocked = true;
            OnLock?.Invoke();
        }
        
    }
    public void Unlock()
    {
        if (IsLocked)
        {
            IsLocked = false;
            OnUnlock?.Invoke();
        }
    }

    private bool EvaluateConditions()
    {
        if (_conditions != null)
        {
            var conditions = _conditions.GetInvocationList();
            for (int i = 0; i < conditions.Length; i++)
            {
                bool result = (bool)conditions[i].DynamicInvoke();
                if (result == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void Awake()
    {
        var conditions = GetComponents<ILockableCondition>();
        foreach (var condition in conditions)
        {
            _conditions += condition.Evaluate;
        }

        conditions = GetComponentsInChildren<ILockableCondition>(true);
        foreach(var condition in conditions)
        {
            _conditions += condition.Evaluate;
        }

        if (IsLocked)
        {
            Lock();
        }
    }

    private void Update()
    {
        // theres probably a better way to handle this.. we shouldn't have to check conditions each frame.
        if (!IsLocked)
        {
            return;
        }

        bool isUnlockable = EvaluateConditions();
        if (isUnlockable) { Unlock(); } 
    }
}
