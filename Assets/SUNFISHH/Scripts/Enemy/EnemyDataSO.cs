using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "CJY/EnemyData", order = 0)]
    public class EnemyDataSO : ScriptableObject
    {
        [field: Header("Info")]
        [field: SerializeField] public string EnemyName { get; private set; } = "Enemy";

        [field: Header("Stat")]
        [field: SerializeField, Min(1)] public int MaxHp { get; private set; } = 3;
        public float MoveSpeed { get; private set; } = 150f;
        [field: SerializeField] public float DetectRange { get; private set; } = 400f;
        [field: SerializeField] public float AttackRange { get; private set; } = 60f;
        [field: SerializeField] public float AttackCoolTime { get; private set; } = 1f;
        [field: SerializeField] public int AttackDamage { get; private set; } = 1;

        [field: Header("Attack")]
        public bool UseAnimationEvents { get; private set; }
        public float AttackWindup { get; private set; } = 0.25f;
        public float AttackDuration { get; private set; } = 0.5f;

        [field: Header("Hit / Dead")]
        [field: SerializeField] public float HitStunTime { get; private set; } = 0.2f;
        [field: SerializeField] public float KnockbackDistance { get; private set; } = 20f;
        [field: SerializeField] public float DeadDestroyDelay { get; private set; } = 0.5f;

        [field: Header("Idle Wander")]
        [field: SerializeField] public float WanderWaitMin { get; private set; } = 0.5f;
        [field: SerializeField] public float WanderWaitMax { get; private set; } = 1.5f;

        [field: Header("Window Range")]
        public float ArriveDistance { get; private set; } = 2f;
    }
}
