using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Afterlife.Core;

[RequireComponent(typeof(Rigidbody2D))]
public class Spider : MonoBehaviour
{
    public Vector2 jumpWindupRange = new Vector2(1, 2);
    public float jumpDuration = .5f;
    public float jumpDistance = 2f;
    public float visionRadius = 5f;
    public LayerMask AvoidanceLayers;

    private float _nextJumpTime;
    private LevelManager _levelManager;
    private Animator _animator;
    private Rigidbody2D _rb;
    private Vector2 _targetPos;
    private TweenerCore<Vector2, Vector2, VectorOptions> _movementTween;

    public void InterruptJump()
    {
        //_movementTween.endValue = transform.position;
        _movementTween.Kill();
        _nextJumpTime = Time.time + Random.Range(jumpWindupRange.x, jumpWindupRange.y) * 1.5f;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _levelManager = FindObjectOfType<LevelManager>();

        _nextJumpTime = Time.time + Random.Range(jumpWindupRange.x, jumpWindupRange.y);

    }
    private void OnDestroy()
    {
        _movementTween.Complete();
    }
    void FixedUpdate()
    {
        if(_nextJumpTime <= Time.time)
        {
            _nextJumpTime = float.MaxValue;
            Vector3 playerPos = _levelManager.PlayerInstance.transform.position;
            bool isPlayerInVisionRange = Vector2.Distance(transform.position, playerPos) <= visionRadius;
            bool isPlayerDead = _levelManager.PlayerHealth.IsDead;
            Vector2 targetDir;

            do
            {
                var randomDir = ShadyMath.AngleSnap(Random.insideUnitCircle.normalized, 45);
                var playerDir = ShadyMath.AngleSnap((playerPos - transform.position).normalized, 45);

                if (!isPlayerDead && isPlayerInVisionRange && Physics2D.Raycast(transform.position, playerDir, jumpDistance, AvoidanceLayers) == false)
                {
                    targetDir = playerDir;
                }
                else
                {
                    targetDir = randomDir;
                }

                _targetPos = (Vector2)transform.position + targetDir * jumpDistance;
            }
            while (Physics2D.Raycast(transform.position, targetDir, jumpDistance, AvoidanceLayers));
           
            _movementTween = _rb.DOMove(_targetPos, jumpDuration);

            _movementTween.onComplete = () =>
            {
                _nextJumpTime = Time.time + Random.Range(jumpWindupRange.x, jumpWindupRange.y);
            };
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Health health))
        {
            health.Damage(1);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _targetPos);
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.color = Color.white;
    }
} 
