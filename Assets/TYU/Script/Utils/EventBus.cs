using System;

namespace TYU.Script
{
    public static class EventBus<T>
    {
        public static event Action<T> OnEvent;
        
        public static void Publish(T data)
        {
            OnEvent?.Invoke(data);
        }

        public static void Subscribe(Action<T> action)
        {
            OnEvent += action;
        }

        public static void Unsubscribe(Action<T> action)
        {
            OnEvent -= action;
        }
    }
}