using System;
using System.Collections.Generic;
using SUNFISHH.Scripts.Player;
using SUNFISHH.Scripts.System;
using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Serializable]
        public class SpawnEntry
        {
            public AbstractEnemy prefab;
            [Min(0f)] public float weight = 1f;
            [Min(0)] public int prewarm = 2;
        }
        
        [SerializeField] private SpawnEntry[] enemies;
        
        [SerializeField] private BoxCollider2D windowRange;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private Transform target;
        [SerializeField] private LayerMask rangeLayer;

        [Header("Spawn")]
        [SerializeField] private int startCount = 1;
        [SerializeField] private int maxCount = 3; // 모든 종류 합쳐서 동시에 살아있을 최대 수
        [SerializeField] private float spawnInterval = 3f;     
        
        private readonly Dictionary<AbstractEnemy, PoolManager<AbstractEnemy>> _pools = new();
        private readonly List<AbstractEnemy> _alive = new();
        private float _nextSpawnTime;

        private int RangeMask => rangeLayer.value != 0 ? rangeLayer.value : LayerMask.GetMask("Range");

        private void Start()
        {
            Resolve();
            CreatePools();

            for (int i = 0; i < startCount; ++i) SpawnRandom();
            _nextSpawnTime = Time.time + spawnInterval;
        }

        private void Update()
        {
            if (spawnInterval <= 0f || Time.time < _nextSpawnTime) return;
            _nextSpawnTime = Time.time + spawnInterval;
            SpawnRandom();
        }

        private void Resolve()
        {
            if (windowRange == null)
            {
                foreach (var col in GetComponentsInChildren<BoxCollider2D>(true))
                {
                    if ((RangeMask & (1 << col.gameObject.layer)) == 0) continue;
                    windowRange = col;
                    break;
                }
            }

            if (spawnParent == null && windowRange != null)
                spawnParent = windowRange.transform.parent != null ? windowRange.transform.parent : windowRange.transform;

            if (target == null)
            {
                var player = FindAnyObjectByType<PlayerAgent>();
                if (player != null) target = player.transform;
            }
        }
        
        private void CreatePools()
        {
            if (enemies == null || spawnParent == null) return;

            foreach (var entry in enemies)
            {
                if (entry == null || entry.prefab == null || _pools.ContainsKey(entry.prefab)) continue;
                
                var pool = new PoolManager<AbstractEnemy>();
                pool.SetPool(entry.prefab, entry.prewarm, spawnParent);
                _pools.Add(entry.prefab, pool);
            }
        }
        
        [ContextMenu("Spawn Random")]
        public AbstractEnemy SpawnRandom()
        {
            int index = PickRandomIndex();
            Debug.Log(index + " 소환됨");
            return index < 0 ? null : SpawnByIndex(index);
        }
        
        public AbstractEnemy SpawnByIndex(int index)
        {
            if (enemies == null || index < 0 || index >= enemies.Length) return null;
            var entry = enemies[index];
            if (entry == null || entry.prefab == null) return null;
            if (entry.prefab.Data == null)
            {
                Debug.LogWarning($"[EnemySpawner] {entry.prefab.name}: EnemyDataSO가 비어있어서 소환 안 함");
                return null;
            }

            _alive.RemoveAll(e => e == null);
            if (_alive.Count >= maxCount) return null;

            if (windowRange == null || spawnParent == null)
            {
                Debug.LogWarning($"[EnemySpawner] {name}: 창 Range를 못 찾음");
                return null;
            }

            if (!_pools.TryGetValue(entry.prefab, out var pool))
            {
                pool = new PoolManager<AbstractEnemy>();
                pool.SetPool(entry.prefab, 0, spawnParent); // [수정] 프리팹만 등록 (미리 생성 X)
                _pools.Add(entry.prefab, pool);
            }

            // [삭제] pool.SetPool(entry.prefab, entry.prewarm, spawnParent);  <- 소환할 때마다 호출돼서 오브젝트가 계속 늘어남
            var enemy = pool.GetPoolItem();
            enemy.gameObject.SetActive(true); // [추가] PoolManager.GetPoolItem은 꺼진 채로 돌려줌 -> 이게 소환 안 되던 원인
            enemy.transform.SetParent(spawnParent, false);
            enemy.SetDespawnHandler(pool.Return);   
            enemy.Init(windowRange, target);
            enemy.SetPosition(enemy.RandomPointInRange());

            enemy.Died += OnEnemyDied;
            _alive.Add(enemy);
            return enemy;
        }

        private int PickRandomIndex()
        {
            if (enemies == null || enemies.Length == 0) return -1;

            float total = 0f;
            foreach (var e in enemies)
                if (e != null && e.prefab != null) total += e.weight;

            if (total <= 0f) return -1;

            float pick = UnityEngine.Random.value * total;
            for (int i = 0; i < enemies.Length; ++i)
            {
                var e = enemies[i];
                if (e == null || e.prefab == null || e.weight <= 0f) continue;
                pick -= e.weight;
                if (pick <= 0f) return i;
            }

            // 부동소수 오차로 끝까지 온 경우 마지막 유효 항목
            for (int i = enemies.Length - 1; i >= 0; --i)
                if (enemies[i] != null && enemies[i].prefab != null && enemies[i].weight > 0f) return i;
            return -1;
        }

        private void OnEnemyDied(AbstractEnemy enemy)
        {
            enemy.Died -= OnEnemyDied;
            _alive.Remove(enemy);
        }
    }
}
