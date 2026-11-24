namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyDeadState : EnemyState
    {
        private bool _despawned;

        public EnemyDeadState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _despawned = false;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            if (_despawned) return;

            bool isEnd = _enemy.Data.UseAnimationEvents ? _endTriggerCalled : ElapsedTime >= _enemy.Data.DeadDestroyDelay;
            if (!isEnd) return;

            _despawned = true;
            _enemy.Despawn();
        }
    }
}
