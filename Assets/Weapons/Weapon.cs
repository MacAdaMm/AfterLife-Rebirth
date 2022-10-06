using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField]
    public float AttackCooldown { get; private set; }

    [SerializeField] private int _damage = 1;

    public bool InUse { get; protected set; }

    public bool AttackStart()
    {
        if (InUse)
        {
            return false;
        }
        else
        {
            StartCoroutine(Attack());
            return true;
        }
    }

    IEnumerator Attack()
    {
        InUse = true;
        Animator.SetTrigger("attack");
        yield return new WaitForSeconds(AttackCooldown);
        InUse = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IHittable hittable))
        {
            hittable.Damage(_damage);
        }
    }
}
