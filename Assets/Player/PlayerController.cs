using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Input;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _flipAnimatorWithOrientation = true;
    
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private MovementController _movementController;

    private Vector2 _moveInput;
    private Health _health;
    
    private void Awake()
    {
        _weaponController = GetComponent<WeaponController>();
        if(TryGetComponent(out _health))
        {
            _health.OnDeath += OnDeath;
        }

    }
    private void OnDestroy()
    {
        if (_health)
        {
            _health.OnDeath -= OnDeath;
        }
    }
    private void Update()
    {
        _moveInput = InputManager.InputActions.Player.Move.ReadValue<Vector2>();
        _movementController.SetMovementInput(_moveInput);

        if(InputManager.InputActions.Player.Attack.WasPerformedThisFrame())
        {
            _weaponController.PreformAttack();
        }
    }
    private void FixedUpdate()
    {
        if (_animator)
        {
            _animator.SetFloat("Speed", _movementController.Velocity.magnitude);

            if (_flipAnimatorWithOrientation)
            {
                if(_moveInput.x > float.Epsilon)
                {
                    _animator.transform.right = Vector3.right;
                }
                else if (_moveInput.x < -float.Epsilon)
                {
                    _animator.transform.right = Vector3.left;
                }
            }
        }
    }

    private void OnDeath()
    {
        _movementController.FreezeMovement(true);

        if (_animator)
        {
            _animator.SetBool("IsDead", true);
            
        }

        enabled = false;
    }
}
