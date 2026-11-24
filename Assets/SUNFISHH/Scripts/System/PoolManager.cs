using System.Collections.Generic;
using UnityEngine;

namespace SUNFISHH.Scripts.System
{
    public class PoolManager<T> where T : Component
    {
        private T _prefab;
        private Transform _parent;
        private readonly Queue<T> _poolQue = new Queue<T>();
        private readonly HashSet<T> _inPool = new HashSet<T>();
        private readonly List<T> _actives = new List<T>();

        public T Prefab => _prefab;
        public int InactiveCount => _poolQue.Count;
        public int ActiveCount => _actives.Count;

        public PoolManager() { }

        public PoolManager(T prefab, int size, Transform parent)
        {
            SetPool(prefab, size, parent);
        }

        public void SetPool(T prefab, int size, Transform parent)
        {
            if (prefab == null)
            {
                Debug.LogError($"[PoolManager<{typeof(T).Name}>] 프리팹이 비어있음");
                return;
            }

            if (_prefab != null && _prefab != prefab)
            {
                Debug.LogError($"[PoolManager<{typeof(T).Name}>] 이미 {_prefab.name} 풀임. 프리팹마다 풀을 따로 만드세요");
                return;
            }

            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < size; i++)
            {
                T obj = InitNewItem();
                obj.gameObject.SetActive(false);
                Enqueue(obj);
            }
        }

        private T InitNewItem()
        {
            return Object.Instantiate(_prefab, _parent);
        }

        public T GetPoolItem()
        {
            if (_prefab == null)
            {
                Debug.LogError($"[PoolManager<{typeof(T).Name}>] SetPool 전에 GetPoolItem 호출됨");
                return null;
            }

            T obj = null;

            while (_poolQue.Count > 0 && obj == null)
            {
                obj = _poolQue.Dequeue();
                if (obj != null) _inPool.Remove(obj);
            }

            if (obj == null)
                obj = InitNewItem();

            obj.gameObject.SetActive(true);
            _actives.Add(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (obj == null) return;
            if (_inPool.Contains(obj)) return;

            _actives.Remove(obj);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parent, false);
            Enqueue(obj);
        }

        public void ReturnAll()
        {
            for (int i = _actives.Count - 1; i >= 0; --i)
                Return(_actives[i]);
            _actives.Clear();
        }

        public void Clear()
        {
            while (_poolQue.Count > 0)
            {
                T obj = _poolQue.Dequeue();
                if (obj != null) Object.Destroy(obj.gameObject);
            }
            _inPool.Clear();
            _actives.Clear();
        }

        private void Enqueue(T obj)
        {
            _poolQue.Enqueue(obj);
            _inPool.Add(obj);
        }
    }
}
