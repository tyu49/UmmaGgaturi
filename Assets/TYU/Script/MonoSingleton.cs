using UnityEngine;

namespace TYU.Script
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"There no Instance of {typeof(T)}");
                    return null;
                }
                return _instance;
            }
        }
        protected bool MakeSingleton() //Call it Awake's first line, copy it "if(!MakeSingleton()) return;"
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return false;
            }
            if (_instance == null)
                _instance = GetComponent<T>();

            return _instance != null;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}