using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Input;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [field: SerializeField]
    public float MaxMoveSpeed { get; set; } = 3f;

    public Vector2 Velocity { get; private set; }

    private Rigidbody2D rb;
    private Health health;
    private Vector2 moveInput;

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
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput * MaxMoveSpeed;
        Velocity = rb.velocity;
    }
}
