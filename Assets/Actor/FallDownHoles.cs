using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownHoles : MonoBehaviour
{
    [SerializeField] private LayerMask _holeLayer;

    private MovementController _movementController;
    private Health _health;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<MovementController>();
        _health = GetComponent<Health>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckForHoles();
    }
    private void CheckForHoles()
    {
        var overlapCollidersCenter = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D collider in overlapCollidersCenter)
        {
            if (collider.gameObject.IsInLayerMask(_holeLayer))
            {
                _movementController.Fall();
                _animator.SetBool("IsFalling", _movementController.CurrentMovementState == MovementController.MovementState.FALLING);
            }
        }

        if (_movementController.CurrentMovementState == MovementController.MovementState.FALLING && !_health.IsDead)
        {
            _health.Kill();
        }
    }
}
