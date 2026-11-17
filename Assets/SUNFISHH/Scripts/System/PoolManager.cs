using System.Collections.Generic;
using TYU.Script;
using UnityEngine;

namespace SUNFISHH.Scripts.System
{
    public class PoolManager<T> where T : MonoSingleton<T>
    {
        public PoolManager<T> Instance { get; private set; }

        private T _prefab;
        private Transform _parent;
        private Queue<T> _poolQue = new Queue<T>();

        private void Awake()
        {
            if(Instance == null) 
                Instance = new PoolManager<T>();
        }

        public void SetPool(T prefab, int size, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < size; i++)
            {
                T obj = InitNewItem();
                obj.gameObject.SetActive(false);
                _poolQue.Enqueue(obj);
            }
        }

        private T InitNewItem()
        {
            return Object.Instantiate(_prefab, _parent);
        }

        public T GetPoolItem()
        {
            T obj;
            if (_poolQue.Count > 0)
            {
                obj = _poolQue.Dequeue();
                return obj;
            }
            else
            {
                obj = InitNewItem();
                return obj;
            }
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _poolQue.Enqueue(obj);
        }
    }
}