using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [field: SerializeField]
    public int Amount { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Health health))
        {
            health.Damage(Amount);
        }
    }
}
