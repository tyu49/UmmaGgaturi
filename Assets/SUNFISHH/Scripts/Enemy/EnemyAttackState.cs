using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyAttackState : EnemyState
    {
        private bool _attacked;

        public EnemyAttackState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _attacked = false;
            _enemy.LastAttackTime = Time.time; 
            if (_enemy.TargetTrm != null) _enemy.FaceTo(_enemy.TargetTrm.position);
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            var data = _enemy.Data;
            if (!data.UseAnimationEvents && !_attacked && ElapsedTime >= data.AttackWindup)
                DoAttack();

            bool isEnd = data.UseAnimationEvents ? _endTriggerCalled : ElapsedTime >= data.AttackDuration;
            if (isEnd)
                _stateMachine.ChangeState(EnemyStateType.Chase);
        }
        
        public override void AnimationActionTrigger()
        {
            DoAttack();
        }

        private void DoAttack()
        {
            if (_attacked) return;
            _attacked = true;
            _enemy.PerformAttack();
        }
    }
}
