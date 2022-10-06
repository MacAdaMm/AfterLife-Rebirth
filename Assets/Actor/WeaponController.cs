using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ShadyPixel;

public class WeaponController : MonoBehaviour
{
    [field: SerializeField]
    public Animator CharacterAnimator { get; private set; }

    [field: SerializeField]
    public Weapon CurrentWeapon { get; private set; }

    [field: SerializeField]
    public Vector2 Orientation { get; private set; } = Vector2.right;

    [SerializeField]
    private Transform _weaponAttachPoint; 

    [Range(0f, 90f)]
    [SerializeField]
    private float _angleSnap = 0f;

    private MovementController _movementController;

    public void PreformAttack()
    {
        if (CurrentWeapon == false) { return; }

        if (CurrentWeapon.AttackStart())
        {
            CharacterAnimator.SetTrigger("attack");
        }
    }

    public void SetWeapon()
    {

    }

    private void Update()
    {
        UpdateOrientation();
        UpdateWeaponAim();
    }

    private void UpdateOrientation()
    {
        if(_movementController.MoveInput.magnitude > float.Epsilon)
        {
            Orientation = _movementController.MoveInput.normalized;
        }
    }

    private void UpdateWeaponAim()
    {
        if(CurrentWeapon == null || CurrentWeapon.InUse) { return; }

        float angle = Orientation.GetAngle(_angleSnap);

        Vector3 normalScale = Vector3.one;
        Vector3 flippedScale = normalScale;
        flippedScale.y = -1f;
        
        if (Orientation.x > 0f) { _weaponAttachPoint.localScale = normalScale; }
        if (Orientation.x < 0f) { _weaponAttachPoint.localScale = flippedScale; }
        

        _weaponAttachPoint.localRotation = Quaternion.Euler(0, 0, angle);
        _weaponAttachPoint.localPosition = -Orientation * 0.0625f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Orientation);
    }

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void OnEnable()
    {
        if (CurrentWeapon)
        {
            CurrentWeapon.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (CurrentWeapon)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }
    }
}