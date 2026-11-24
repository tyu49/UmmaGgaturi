using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyHitState : EnemyState
    {
        private Vector2 _from;
        private Vector2 _to;

        public EnemyHitState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _from = _enemy.transform.position;
            _to = _enemy.ClampInRange(_from + _enemy.LastHitDir * _enemy.Data.KnockbackDistance);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            float stun = _enemy.Data.HitStunTime;
            float t = stun <= 0f ? 1f : Mathf.Clamp01(ElapsedTime / stun);
            _enemy.SetPosition(Vector2.Lerp(_from, _to, t));

            bool isEnd = _enemy.Data.UseAnimationEvents ? _endTriggerCalled : t >= 1f;
            if (!isEnd) return;

            _stateMachine.ChangeState(_enemy.CanDetectTarget() ? EnemyStateType.Chase : EnemyStateType.Idle);
        }
    }
}
