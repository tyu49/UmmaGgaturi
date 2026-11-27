using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SUNFISHH.Scripts.Player
{
    public class PlayerAgent : AbstractAgent
    {
        [Header("Settings")] 
        [SerializeField] private float _followDur = 0.3f;
        private Vector2 _mousePos;
        
        [Header("Change Range")]
        [SerializeField] private List<BoxCollider2D> _moveRange;
        private BoxCollider2D _curRange;
        [SerializeField] private LayerMask _windowLayer;
        [SerializeField] private float _rangeSearchDistance;

        private void Update()
        {
            Move();
            KeyBoardValue();
        }
        

        protected override void Move()
        {
            _mousePos = Mouse.current.position.ReadValue();
            //Tween.Position(transform, _mousePos, _followDur);
            
            Vector2 next = Vector2.Lerp(transform.position, ClampRange(_mousePos), _followDur);
            transform.position = next;
        }
        
        
        protected Vector2 ClampRange(Vector2 nextPos)
        {
            Vector2 pos = nextPos;
            
            foreach (var range in _moveRange)
            {
                if (range.OverlapPoint(pos))
                {
                    if(_curRange != null)
                    {
                        if (_curRange != range)
                        {
                            // 현재 범위가 아니면
                            continue;
                        }
                    }
                    _curRange = range;
                    return pos;
                }
                
            }
            
            Bounds bounds = _curRange.bounds;

            float left = bounds.min.x + _rangeOffset.x;
            float right = bounds.max.x - _rangeOffset.x;
            float bottom = bounds.min.y + _rangeOffset.y;
            float top = bounds.max.y - _rangeOffset.y;

            pos.x = left <= right ? Mathf.Clamp(pos.x, left, right) : bounds.center.x;
            pos.y = bottom <= top ? Mathf.Clamp(pos.y, bottom, top) : bounds.center.y;

            return pos;
        }       
        
        // 수정 필요
        protected void TryChangeRange(Vector2 dir)
        {
            if (_curRange == null) return;

            Vector2 origin = transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, _rangeSearchDistance, _windowLayer);
            Debug.DrawRay(origin, dir * _rangeSearchDistance, Color.red, 0.3f);

            foreach (var hit in hits)
            {
                if (hit.collider != _curRange)
                {
                    _curRange = hit.collider.gameObject.GetComponent<BoxCollider2D>();
                }
            }

        }

        private void KeyBoardValue()
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.up);
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.left);
            }

            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.down);
            }

            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                TryChangeRange(Vector2.right);
            }
        }
    }
}