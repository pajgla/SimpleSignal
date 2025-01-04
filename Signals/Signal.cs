using System;

namespace Core.Signals
{
    public class Signal<T>
    {
        Action<T> m_Listeners = delegate { };

        public void Subscribe(Action<T> listener)
        {
            m_Listeners += listener;
        }

        public void Unsubscribe(Action<T> listener)
        {
            m_Listeners -= listener;
        }

        public void Dispatch(T data)
        {
            m_Listeners.Invoke(data);
        }
    }
}

