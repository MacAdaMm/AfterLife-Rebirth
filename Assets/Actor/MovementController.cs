using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private const float IDLE_THRESHOLD = 0.1f;
    public enum MovementState { IDLE, MOVING, FALLING, DASHING }

    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private Rigidbody2D _rigidBody;
    private bool _isFrozen;

    public Vector2 MoveInput { get; private set; }
    public Vector2 Velocity { get; private set; }
    public MovementState CurrentMovementState { get; private set; }
    
    public void SetMovementInput(Vector2 moveInput)
    {
        MoveInput = moveInput;

        if (moveInput.magnitude <= IDLE_THRESHOLD)
        {
            MoveInput = Vector2.zero;
        }
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = MoveInput * _moveSpeed;

        if (_isFrozen)
        {
            _rigidBody.velocity = Vector2.zero;
        }

        Velocity = _rigidBody.velocity;
    }

    public void FreezeMovement(bool state)
    {
        _isFrozen = state;
    }

    public void Fall()
    {
        CurrentMovementState = MovementState.FALLING;
        FreezeMovement(true);
    }
}
