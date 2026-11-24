using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public abstract class EnemyState
    {
        protected readonly AbstractEnemy _enemy;
        protected readonly EnemyStateMachine _stateMachine;

        protected readonly int _animBoolHash;
        protected bool _endTriggerCalled;

        private float _enterTime;
        protected float ElapsedTime => Time.time - _enterTime;

        protected EnemyState(AbstractEnemy enemy, EnemyStateMachine stateMachine, string animBoolName)
        {
            _enemy = enemy;
            _stateMachine = stateMachine;
            _animBoolHash = Animator.StringToHash(animBoolName);
        }

        public virtual void Enter()
        {
            _enemy.SetAnimBool(_animBoolHash, true);
            _endTriggerCalled = false;
            _enterTime = Time.time;
        }

        public virtual void Exit()
        {
            _enemy.SetAnimBool(_animBoolHash, false);
        }

        public virtual void UpdateState()
        {
        }
        
        public void AnimationEndTrigger()
        {
            _endTriggerCalled = true;
        }
        
        public virtual void AnimationActionTrigger()
        {
        }
    }
}
