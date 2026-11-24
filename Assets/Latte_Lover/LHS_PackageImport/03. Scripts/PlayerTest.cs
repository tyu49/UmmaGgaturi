using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Latte_Lover.LHS_PackageImport._03._Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerTest : MonoBehaviour
    {
        [SerializeField]private float _speed;
        
        private Rigidbody2D _rb;
        private Vector2 _moveDir;
        private SpriteRenderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                EffectManager.instance.PlayFlash(_renderer,Color.white,3,1f,0.7f, EffectManager.FlashMode.InstantFade);
            }
            
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveDir * _speed;
        }

        private void OnMove(InputValue value)
        {
            _moveDir = value.Get<Vector2>();
        }
    }
}