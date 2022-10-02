using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [field:SerializeField]
    public float AttackCooldown { get; private set; }

    public void AttackStart()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        var hitbox = GetComponent<BoxCollider2D>().gameObject;
        hitbox.SetActive(true);
        yield return new WaitForSeconds(AttackCooldown);
        hitbox.SetActive(false);
    }
}
