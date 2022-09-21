using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Input;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [field:SerializeField]
    public float MaxMoveSpeed { get; set; }

    [field: SerializeField]
    public Vector2 Velocitiy { get; private set; }

    Rigidbody2D rb;

    Vector2 stick;

    private void Awake()
    {
        if(TryGetComponent(out Health health))
        {
            health.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        enabled = false;
        rb.isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        stick = InputManager.InputActions.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = stick * MaxMoveSpeed;
        Velocitiy = rb.velocity;
    }
}
