using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyIdleState : EnemyState
    {
        private bool _hasDest;
        private Vector2 _dest;
        private float _waitUntil;

        public EnemyIdleState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _hasDest = false;
            ResetWait();
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (_enemy.CanDetectTarget())
            {
                _stateMachine.ChangeState(EnemyStateType.Chase);
                return;
            }

            if (!_hasDest)
            {
                if (Time.time < _waitUntil) return;
                _dest = _enemy.RandomPointInRange();
                _hasDest = true;
            }
            
            if (_enemy.MoveToward(_dest))
            {
                _hasDest = false;
                ResetWait();
            }
        }

        private void ResetWait()
        {
            _waitUntil = Time.time + Random.Range(_enemy.Data.WanderWaitMin, _enemy.Data.WanderWaitMax);
        }
    }
}
