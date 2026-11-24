namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyChaseState : EnemyState
    {
        public EnemyChaseState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void UpdateState()
        {
            base.UpdateState();
            
            if (_enemy.ShouldLoseTarget())
            {
                _stateMachine.ChangeState(EnemyStateType.Idle);
                return;
            }
            
            if (_enemy.DistanceToTarget() <= _enemy.Data.AttackRange)
            {
                _enemy.FaceTo(_enemy.TargetTrm.position);
                if (_enemy.CanAttack)
                    _stateMachine.ChangeState(EnemyStateType.Attack);
                return;
            }
            
            _enemy.MoveToward(_enemy.TargetTrm.position);
        }
    }
}
