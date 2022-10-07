using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Action OnAttackPerformed;
    public Action OnAttackFinished;

    [SerializeField] private Animator _animator;
    [SerializeField] private int _damage = 1;

    public bool InUse { get; private set; }

    // Check current state and trigger animations
    public void Attack()
    {
        if (InUse) { return; }

        InUse = true;
        _animator.SetTrigger("attack");
        OnAttackPerformed?.Invoke();
    }

    // Called back from the animator on last frame of weapon attack or whenever you want cooldown to end.
    public void FinishAttack()
    {
        if (InUse == false) { return; }

        InUse = false;
        OnAttackFinished?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHittable hittable))
        {
            hittable.Damage(_damage);
        }
    }
}
