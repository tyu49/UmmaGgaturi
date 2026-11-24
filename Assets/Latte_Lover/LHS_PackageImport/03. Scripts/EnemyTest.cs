using UnityEngine;
using UnityEngine.InputSystem;

namespace Latte_Lover.LHS_PackageImport._03._Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyTest : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Follow Settings")]
        [SerializeField] private bool followOnStart = false;
        [SerializeField, Min(0.01f)] private float smoothTime = 0.3f;
        [SerializeField, Min(0.01f)] private float maxSpeed = 5f;
        [SerializeField, Min(0f)] private float stopDistance = 0.1f;

        [Header("After Image Settings")]
        [SerializeField, Min(0.01f)] private float afterImageInterval = 0.05f;
        [SerializeField, Min(0.01f)] private float afterImageLifetime = 0.5f;
        [SerializeField, Range(0f, 1f)] private float afterImageAlpha = 0.7f;
        [SerializeField] private Color afterImageColor = Color.black;

        private Rigidbody2D _rb;
        private Vector2 _velocity;
        private bool _isFollowing;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (followOnStart)
                StartFollow();
        }

        private void Update()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.zKey.wasPressedThisFrame)
                StartFollow();

            if (Keyboard.current.xKey.wasPressedThisFrame)
                StopFollow();
        }

        private void FixedUpdate()
        {
            if (!_isFollowing || target == null) return;

            Vector2 currentPosition = _rb.position;
            Vector2 targetPosition = target.position;

            if (Vector2.Distance(currentPosition, targetPosition) <= stopDistance)
            {
                _velocity = Vector2.zero;
                return;
            }

            Vector2 nextPosition = Vector2.SmoothDamp(
                currentPosition,
                targetPosition,
                ref _velocity,
                smoothTime,
                maxSpeed,
                Time.fixedDeltaTime
            );

            _rb.MovePosition(nextPosition);
        }

        public void StartFollow()
        {
            if (_isFollowing || target == null) return;

            _isFollowing = true;

            if (spriteRenderer != null)
            {
                EffectManager.instance.StartAfterImage(
                    spriteRenderer,
                    afterImageInterval,
                    afterImageLifetime,
                    afterImageColor,
                    afterImageAlpha);
            }
        }

        public void StopFollow()
        {
            if (!_isFollowing) return;

            _isFollowing = false;
            _velocity = Vector2.zero;

            EffectManager.instance.StopAfterImage(spriteRenderer);
        }

        private void OnDisable()
        {
            if(EffectManager.instance!=null) StopFollow();
        }
    }
}
