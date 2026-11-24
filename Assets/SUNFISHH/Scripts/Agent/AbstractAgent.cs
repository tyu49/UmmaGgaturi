using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace SUNFISHH.Scripts.Player
{
    public class AbstractAgent : MonoBehaviour
    {
        [Header("Settings")] 
        protected AgentMover moveCompo;
        protected AgentRenderer renderCompo;
        
        [SerializeField] protected Vector2 _rangeOffset;

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