namespace UnityEngine.EventSystems
{
    /// <summary>
    /// 抽象事件数据类
    /// </summary>
    public abstract class AbstractEventData
    {
        /// <summary>
        /// 是否使用过
        /// </summary>
        protected bool m_Used;

        /// <summary>
        /// 重置函数
        /// </summary>
        public virtual void Reset()
        {
            m_Used = false;
        }

        /// <summary>
        /// 使用函数
        /// </summary>
        public virtual void Use()
        {
            m_Used = true;
        }

        /// <summary>
        /// 是否使用过
        /// </summary>
        public virtual bool used
        {
            get { return m_Used; }
        }
    }

    /// <summary>
    /// 基础事件函数
    /// </summary>
    public class BaseEventData : AbstractEventData
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        private readonly EventSystem m_EventSystem;
        public BaseEventData(EventSystem eventSystem)
        {
            m_EventSystem = eventSystem;
        }
        /// <summary>
        /// 当前的输入模块
        /// </summary>
        public BaseInputModule currentInputModule
        {
            get { return m_EventSystem.currentInputModule; }
        }
        /// <summary>
        /// 设置当前选择的对象
        /// </summary>
        public GameObject selectedObject
        {
            get { return m_EventSystem.currentSelectedGameObject; }
            set { m_EventSystem.SetSelectedGameObject(value, this); }
        }
    }
}
