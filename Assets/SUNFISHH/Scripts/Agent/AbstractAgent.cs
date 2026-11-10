using System;
using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            moveCompo = GetComponent<AgentMover>();
            renderCompo = GetComponentInChildren<AgentRenderer>();
        }

        private void LateUpdate()
        {
            ClampRange();
        }

        private void ClampRange()
        {
            Vector2 pos = new Vector2(transform.position.x + _rangeOffset.x, transform.position.y +  _rangeOffset.y);
            
            foreach (var range in _moveRange)
            {
                if (range.OverlapPoint(pos))
                {
                    if(_curRange != null)
                    {
                        if (!_curRange == range)
                        {
                            // 현재 범위가 아니면
                            Debug.Log($"범위안은 맞는데 현재 범위가 아님 {_curRange} != {range}");
                            continue;
                        }
                    }
                    Debug.Log("범위가 존재하지 않음");
                    _curRange = range;
                    return;
                }
            }
            Vector2 clamp = _curRange.ClosestPoint(pos) - _rangeOffset;
            transform.position = clamp;
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