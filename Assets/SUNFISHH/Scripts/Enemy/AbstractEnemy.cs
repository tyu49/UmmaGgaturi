using System;
using System.Collections.Generic;
using SUNFISHH.Scripts.Player;
using UnityEngine;

namespace SUNFISHH.Scripts.Enemy
{
    public abstract class AbstractEnemy : AbstractAgent
    {
        [Header("Data")]
        [SerializeField] private EnemyDataSO data;
        public EnemyDataSO Data => data;

        [Header("Window Range")]
        [SerializeField] private LayerMask rangeLayer;

        [Header("Scene Test")]
        [SerializeField] private bool initOnStart;

        public int Hp { get; private set; }
        public bool IsDead { get; private set; }
        public Transform TargetTrm { get; private set; }
        public BoxCollider2D Range { get; private set; }
        public Animator AnimCompo { get; private set; }
        public EnemyStateMachine StateMachine { get; private set; }
        public float LastAttackTime { get; set; } = float.NegativeInfinity;
        public Vector2 LastHitDir { get; private set; }

        public bool CanAttack => Time.time >= LastAttackTime + data.AttackCoolTime;

        public event Action<AbstractEnemy> Attacked;   // 공격 판정 순간 
        public event Action<AbstractEnemy> Died;       

        private Action<AbstractEnemy> _despawnHandler;
        private readonly HashSet<int> _animBoolParams = new();
        private readonly List<BoxCollider2D> _otherRanges = new();  
        private int _cachedSiblingCount = -1;

        #region 초기화
        
        public void Init(BoxCollider2D range, Transform target)
        {
            if (data == null)
            {
                Debug.LogError($"[{name}] EnemyDataSO가 비어있음");
                return;
            }

            Range = range;
            TargetTrm = target;
            Hp = data.MaxHp;
            IsDead = false;
            LastAttackTime = float.NegativeInfinity;

            CacheAnimator();

            StateMachine = new EnemyStateMachine();
            SetUpStates(StateMachine);
            StateMachine.Initialize(EnemyStateType.Idle);

            SetPosition(ClampInRange(transform.position));
        }
        
        protected virtual void SetUpStates(EnemyStateMachine machine)
        {
            machine.AddState(EnemyStateType.Idle, new EnemyIdleState(this, machine, "Idle"));
            machine.AddState(EnemyStateType.Chase, new EnemyChaseState(this, machine, "Chase"));
            machine.AddState(EnemyStateType.Attack, new EnemyAttackState(this, machine, "Attack"));
            machine.AddState(EnemyStateType.Hit, new EnemyHitState(this, machine, "Hit"));
            machine.AddState(EnemyStateType.Dead, new EnemyDeadState(this, machine, "Dead"));
        }

        protected virtual void Start()
        {
            if (!initOnStart || StateMachine != null) return;

            var range = FindWindowRange(transform, RangeMask);
            var player = FindAnyObjectByType<PlayerAgent>();
            if (range == null)
            {
                Debug.LogWarning($"[{name}] 부모 창에서 Range 콜라이더를 못 찾음");
                return;
            }
            Init(range, player != null ? player.transform : null);
        }

        private void CacheAnimator()
        {
            AnimCompo = GetComponentInChildren<Animator>();
            _animBoolParams.Clear();
            if (AnimCompo == null || AnimCompo.runtimeAnimatorController == null) return;

            foreach (var p in AnimCompo.parameters)
                if (p.type == AnimatorControllerParameterType.Bool)
                    _animBoolParams.Add(p.nameHash);
        }

        #endregion

        protected virtual void Update()
        {
            if (StateMachine == null) return;
            if (Range == null) return; // 창이 사라짐

            StateMachine.CurrentState.UpdateState();
            
            SetPosition(ClampInRange(transform.position));
        }

        #region 창 범위

        public int RangeMask => rangeLayer.value != 0 ? rangeLayer.value : LayerMask.GetMask("Range");

        // 콜라이더의 월드 사각형을 transform에서 바로 계산 (bound 부분 수정?)
        public static Rect GetWorldRect(BoxCollider2D col)
        {
            Transform t = col.transform;
            Vector2 half = col.size * 0.5f;
            Vector3 a = t.TransformPoint(col.offset - half);
            Vector3 b = t.TransformPoint(col.offset + half);
            return Rect.MinMaxRect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
        }
        
        public Vector2 ClampInRange(Vector2 pos)
        {
            if (Range == null) return pos;

            Rect r = GetWorldRect(Range);
            Vector2 pad = _rangeOffset;
            float left = r.xMin + pad.x;
            float right = r.xMax - pad.x;
            float bottom = r.yMin + pad.y;
            float top = r.yMax - pad.y;

            pos.x = left <= right ? Mathf.Clamp(pos.x, left, right) : r.center.x;
            pos.y = bottom <= top ? Mathf.Clamp(pos.y, bottom, top) : r.center.y;
            return pos;
        }

        public Vector2 RandomPointInRange()
        {
            if (Range == null) return transform.position;

            Rect r = GetWorldRect(Range);
            Vector2 pad = _rangeOffset;
            var p = new Vector2(
                UnityEngine.Random.Range(r.xMin + pad.x, r.xMax - pad.x),
                UnityEngine.Random.Range(r.yMin + pad.y, r.yMax - pad.y));
            return ClampInRange(p);
        }
        
        public bool IsInMyWindow(Vector2 point)
        {
            if (Range == null || !GetWorldRect(Range).Contains(point)) return false;

            Transform myWindow = WindowOf(Range);
            RefreshOtherRanges(myWindow);

            foreach (var other in _otherRanges)
            {
                if (other == null || !other.gameObject.activeInHierarchy) continue; // 닫힌 창 무시
                if (!IsAbove(WindowOf(other), myWindow)) continue;
                if (GetWorldRect(other).Contains(point)) return false;
            }
            return true;
        }
        
        private void RefreshOtherRanges(Transform myWindow)
        {
            Transform parent = myWindow.parent;
            if (parent == null || parent.childCount == _cachedSiblingCount) return;

            _cachedSiblingCount = parent.childCount;
            _otherRanges.Clear();
            int mask = RangeMask;

            foreach (Transform sibling in parent)
            {
                if (sibling == myWindow) continue;
                foreach (var col in sibling.GetComponentsInChildren<BoxCollider2D>(true))
                {
                    if ((mask & (1 << col.gameObject.layer)) == 0) continue;
                    _otherRanges.Add(col);
                    break;
                }
            }
        }
        
        private static Transform WindowOf(Collider2D col)
        {
            return col.transform.parent != null ? col.transform.parent : col.transform;
        }
        
        private static bool IsAbove(Transform a, Transform b)
        {
            if (a == b || a.parent != b.parent) return false;
            return a.GetSiblingIndex() > b.GetSiblingIndex();
        }
        
        public static BoxCollider2D FindWindowRange(Transform from, int mask)
        {
            for (Transform t = from; t != null; t = t.parent)
            {
                foreach (var col in t.GetComponentsInChildren<BoxCollider2D>(true))
                {
                    if (col.transform == from || col.transform.IsChildOf(from)) continue; // 자기 자신 제외
                    if ((mask & (1 << col.gameObject.layer)) != 0) return col;
                }
            }
            return null;
        }

        #endregion

        #region 이동
        
        public bool MoveToward(Vector2 destination)
        {
            Vector2 cur = transform.position;
            Vector2 dest = ClampInRange(destination);
            Vector2 diff = dest - cur;

            float arrive = data.ArriveDistance;
            if (diff.sqrMagnitude <= arrive * arrive) return true;

            FaceTo(dest);
            SetPosition(Vector2.MoveTowards(cur, dest, data.MoveSpeed * Time.deltaTime));
            return ((Vector2)transform.position - dest).sqrMagnitude <= arrive * arrive;
        }
        
        public void FaceTo(Vector2 lookPos)
        {
            if (renderCompo == null) return;
            renderCompo.Flip(lookPos.x - transform.position.x);
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        }

        #endregion

        #region 타겟

        public float DistanceToTarget()
        {
            return TargetTrm == null ? float.MaxValue : Vector2.Distance(transform.position, TargetTrm.position);
        }

        public bool CanDetectTarget()
        {
            return TargetTrm != null
                   && IsInMyWindow(TargetTrm.position)
                   && DistanceToTarget() <= data.DetectRange;
        }

        public bool ShouldLoseTarget()
        {
            return TargetTrm == null || !IsInMyWindow(TargetTrm.position);
        }

        #endregion

        #region 공격 / 피격 / 사망

        // EnemyAttackState에서 호출
        public virtual void PerformAttack()
        {
            if (TargetTrm == null || !IsInMyWindow(TargetTrm.position)) return;
            if (DistanceToTarget() > data.AttackRange * 1.1f) return;

            Attacked?.Invoke(this);
        }

        // 플레이어 공격 쪽에서 호출
        public virtual void TakeDamage(int damage, Vector2 hitFrom)
        {
            if (IsDead || StateMachine == null) return;

            Hp -= damage;
            Vector2 dir = (Vector2)transform.position - hitFrom;
            LastHitDir = dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector2.zero;

            if (Hp <= 0)
            {
                IsDead = true;
                StateMachine.ChangeState(EnemyStateType.Dead);
                Died?.Invoke(this);
            }
            else
            {
                StateMachine.ChangeState(EnemyStateType.Hit);
            }
            
            Debug.Log($"{Data.EnemyName} HP -{damage}");
        }
        
        public void SetDespawnHandler(Action<AbstractEnemy> handler)
        {
            _despawnHandler = handler;
        }
        
        public virtual void Despawn()
        {
            StateMachine = null; 

            if (_despawnHandler != null) _despawnHandler.Invoke(this);
            else Destroy(gameObject);
        }

        #endregion

        #region 애니메이션

        public void SetAnimBool(int hash, bool value)
        {
            if (AnimCompo == null || !_animBoolParams.Contains(hash)) return; 
            AnimCompo.SetBool(hash, value);
        }
        
        public void AnimationEndTrigger() => StateMachine?.CurrentState.AnimationEndTrigger();
        public void AnimationActionTrigger() => StateMachine?.CurrentState.AnimationActionTrigger();

        #endregion
        
        private void OnDrawGizmos()
        {
            if (data == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.AttackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.DetectRange);
        }
    }
}
