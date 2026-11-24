using System.Collections.Generic;
using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyStateMachine
    {
        public EnemyState CurrentState { get; private set; }
        public EnemyStateType CurrentType { get; private set; }

        private readonly Dictionary<EnemyStateType, EnemyState> _stateDic = new();

        public void AddState(EnemyStateType type, EnemyState state)
        {
            _stateDic[type] = state; 
        }

        public void Initialize(EnemyStateType type)
        {
            CurrentType = type;
            CurrentState = _stateDic[type];
            CurrentState.Enter();
        }

        public void ChangeState(EnemyStateType type)
        {
            if (!_stateDic.TryGetValue(type, out var next))
            {
                Debug.LogWarning($"[EnemyStateMachine] {type} 상태가 등록되지 않음");
                return;
            }

            CurrentState?.Exit();
            CurrentType = type;
            CurrentState = next;
            CurrentState.Enter();
        }
    }
}
