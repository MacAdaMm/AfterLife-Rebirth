using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ShadyPixel;

public class WeaponController : MonoBehaviour
{
    private const float ATTACK_INPUT_THRESHOLD = 0.25f;

    [field: SerializeField]
    public Animator CharacterAnimator { get; private set; }

    [field: SerializeField]
    public Weapon CurrentWeapon { get; private set; }

    [field: SerializeField]
    public Vector2 Orientation { get; private set; } = Vector2.right;

    [SerializeField]
    private Transform _weaponAttachPoint; 

    private MovementController _movementController;
    private float _lastAttackInputTime = float.MinValue;

    public void Attack()
    {
        if (CurrentWeapon == false) { return; }
        _lastAttackInputTime = Time.time;
    }

    private void Update()
    {
        UpdateOrientation();
        UpdateWeaponAim();
        ProcessAttack();
    }

    private void UpdateOrientation()
    {
        if(_movementController.MoveInput.magnitude > 0.1f)
        {
            Orientation = _movementController.MoveInput.normalized;
        }
    }

    private void UpdateWeaponAim()
    {
        if(CurrentWeapon == null || CurrentWeapon.InUse) { return; }

        float angle = Orientation.GetAngle();

        Vector3 normalScale = Vector3.one;
        Vector3 flippedScale = normalScale;
        flippedScale.y = -1f;
        
        if (Orientation.x > 0f) { _weaponAttachPoint.localScale = normalScale; }
        if (Orientation.x < 0f) { _weaponAttachPoint.localScale = flippedScale; }
        

        _weaponAttachPoint.localRotation = Quaternion.Euler(0, 0, angle);
        _weaponAttachPoint.localPosition = -Orientation * 0.0625f;
    }

    private void ProcessAttack()
    {
        if (CurrentWeapon == null || CurrentWeapon.InUse) { return; }

        if(_lastAttackInputTime + ATTACK_INPUT_THRESHOLD > Time.time)
        {
            _lastAttackInputTime = float.MinValue;
            CurrentWeapon.Attack();
        }
    }

    private void OnWeaponAttackPerformed()
    {
        CharacterAnimator.SetTrigger("attack");
        _movementController.FreezeMovement(true);
    }

    private void OnWeaponAttackFinished()
    {
        _movementController.FreezeMovement(false);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Orientation);
    }

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        if (CurrentWeapon != null)
        {
            CurrentWeapon.OnAttackPerformed += OnWeaponAttackPerformed;
            CurrentWeapon.OnAttackFinished += OnWeaponAttackFinished;
        }
    }
}