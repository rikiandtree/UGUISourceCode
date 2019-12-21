namespace UnityEngine.EventSystems
{
    /// <summary>
    /// �����¼�������
    /// </summary>
    public abstract class AbstractEventData
    {
        /// <summary>
        /// �Ƿ�ʹ�ù�
        /// </summary>
        protected bool m_Used;

        /// <summary>
        /// ���ú���
        /// </summary>
        public virtual void Reset()
        {
            m_Used = false;
        }

        /// <summary>
        /// ʹ�ú���
        /// </summary>
        public virtual void Use()
        {
            m_Used = true;
        }

        /// <summary>
        /// �Ƿ�ʹ�ù�
        /// </summary>
        public virtual bool used
        {
            get { return m_Used; }
        }
    }

    /// <summary>
    /// �����¼�����
    /// </summary>
    public class BaseEventData : AbstractEventData
    {
        /// <summary>
        /// �¼�ϵͳ
        /// </summary>
        private readonly EventSystem m_EventSystem;
        public BaseEventData(EventSystem eventSystem)
        {
            m_EventSystem = eventSystem;
        }
        /// <summary>
        /// ��ǰ������ģ��
        /// </summary>
        public BaseInputModule currentInputModule
        {
            get { return m_EventSystem.currentInputModule; }
        }
        /// <summary>
        /// ���õ�ǰѡ��Ķ���
        /// </summary>
        public GameObject selectedObject
        {
            get { return m_EventSystem.currentSelectedGameObject; }
            set { m_EventSystem.SetSelectedGameObject(value, this); }
        }
    }
}
