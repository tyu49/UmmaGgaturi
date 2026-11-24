using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public class EnemyAnimationTrigger : MonoBehaviour
    {
        private AbstractEnemy _enemy;

        private void Awake()
        {
            _enemy = GetComponentInParent<AbstractEnemy>();
        }
        
        public void AttackTrigger()
        {
            if (_enemy != null) _enemy.AnimationActionTrigger();
        }
        
        public void EndTrigger()
        {
            if (_enemy != null) _enemy.AnimationEndTrigger();
        }
    }
}
