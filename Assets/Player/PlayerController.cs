using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Input;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const float IDLE_THRESHOLD = 0.1f;

    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private bool flipAnimatorWithOrientation = true;
    [SerializeField] private LayerMask _holeLayer;

    public Vector2 Velocity { get; private set; }

    private Rigidbody2D rb;
    private Health health;
    private Vector2 moveInput;
    private bool _isOverHole;

    private void Awake()
    {
        if(TryGetComponent(out health))
        {
            health.OnDeath += OnDeath;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDeath()
    {
        rb.velocity = Vector2.zero;
        rb.simulated = false;

        if (animator)
        {
            animator.SetBool("IsDead", true);
        }

        enabled = false;
    }

    private void OnDestroy()
    {
        if (health)
        {
            health.OnDeath -= OnDeath;
        }
    }

    private void Update()
    {
        moveInput = InputManager.InputActions.Player.Move.ReadValue<Vector2>();
        if(moveInput.magnitude < IDLE_THRESHOLD)
        {
            moveInput = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        CheckForHoles();

        rb.velocity = moveInput * moveSpeed;
        Velocity = rb.velocity;

        if (animator)
        {
            animator.SetFloat("Speed", Velocity.magnitude);

            if (flipAnimatorWithOrientation)
            {
                if(moveInput.x > float.Epsilon)
                {
                    animator.transform.right = Vector3.right;
                }
                else if (moveInput.x < -float.Epsilon)
                {
                    animator.transform.right = Vector3.left ;
                }
            }
        }
    }

    private void CheckForHoles()
    {
        _isOverHole = false;

        var collider2d = GetComponent<Collider2D>();
        var overlapCollidersCenter = Physics2D.OverlapPointAll(transform.position);
        foreach(Collider2D collider in overlapCollidersCenter)
        {
            if (collider.gameObject.IsInLayerMask(_holeLayer))
            {
                _isOverHole = true;
            }
        }

        animator.SetBool("IsFalling", _isOverHole);

        if (_isOverHole && health.CurrentHealth > 0)
        {
            health.Kill();
        }
    }
}
