using UnityEngine;

internal interface IHittable
{
    void Damage(int damage = 1);
}