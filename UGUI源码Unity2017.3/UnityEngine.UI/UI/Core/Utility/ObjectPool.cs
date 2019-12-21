using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    //对象池
    internal class ObjectPool<T> where T : new()
    {
        //栈
        private readonly Stack<T> m_Stack = new Stack<T>();
        //一个参数，空返回的委托（get时调用）
        private readonly UnityAction<T> m_ActionOnGet;
        //一个参数，空返回的委托（release时调用）
        private readonly UnityAction<T> m_ActionOnRelease;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T element;
            //栈空则new元素，总数量++,否则移除头部元素
            if (m_Stack.Count == 0)
            {
                element = new T();
                countAll++;
            }
            else
            {
                element = m_Stack.Pop();
            }
            //获得时委托函数执行
            if (m_ActionOnGet != null)
                m_ActionOnGet(element);
            return element;
        }

        //释放元素
        public void Release(T element)
        {
            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            if (m_ActionOnRelease != null)
                m_ActionOnRelease(element);
            m_Stack.Push(element);
        }
    }
}
