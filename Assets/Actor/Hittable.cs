using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Hittable : MonoBehaviour, IHittable
{
    [SerializeField] private UnityEvent _onHit;

    public void Damage(int damage)
    {
        _onHit?.Invoke();
    }
}
