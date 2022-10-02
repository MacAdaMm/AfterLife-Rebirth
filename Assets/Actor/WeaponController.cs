using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField]
    public Weapon CurrentWeapon { get; private set; }

    public void PreformAttack()
    {
        throw new NotImplementedException();
    }
    public void SetWeapon()
    {

    }
}
