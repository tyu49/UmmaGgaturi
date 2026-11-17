using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace SUNFISHH.Scripts.Player
{
    public class AbstractAgent : MonoBehaviour
    {
        [Header("Settings")] 
        public AgentMover moveCompo;
        public AgentRenderer renderCompo;
        
        [SerializeField] private List<BoxCollider2D> _moveRange;

        [SerializeField] private Vector2 _rangeOffset;
        private BoxCollider2D _curRange;

        [Header("Change Range")]
        [SerializeField] private LayerMask _windowLayer;
        [SerializeField] private float _rangeSearchDistance;
        

        private void Awake()
        {
            moveCompo = GetComponent<AgentMover>();
            renderCompo = GetComponentInChildren<AgentRenderer>();
        }

        private void LateUpdate()
        {
        }

        protected virtual void Move()
        {
            moveCompo.Move();
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
        
        /*public bool CheckRange(Vector2 nextMovePos)
        
        {
            foreach (var range in _moveRange)
            {
                if (range.bounds.Contains(nextMovePos)) return true;

                /*if (range.bounds.Contains(colliderCompo.bounds.min) &&
                    range.bounds.Contains(colliderCompo.bounds.max)) return;#1#
            }
            
            // 범위내가 아닐때 안으로 이동하는 로직
            return false;

        }*/
    }
}