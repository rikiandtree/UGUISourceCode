using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
    [AddComponentMenu("Event/Event System")]
    public class EventSystem : UIBehaviour
    {
        //����ϵͳ
        private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();
        //��ǰ����ϵͳ
        private BaseInputModule m_CurrentInputModule;
        //�¼�ϵͳ�б�
        private  static List<EventSystem> m_EventSystems = new List<EventSystem>();
        //��ǰ�¼�
        public static EventSystem current
        {
            get { return m_EventSystems.Count > 0 ? m_EventSystems[0] : null; }
            set
            {
                int index = m_EventSystems.IndexOf(value);

                if (index >= 0)
                {
                    m_EventSystems.RemoveAt(index);
                    m_EventSystems.Insert(0, value);
                }
            }
        }

        [SerializeField]
        [FormerlySerializedAs("m_Selected")]
        private GameObject m_FirstSelected;

        [SerializeField]
        private bool m_sendNavigationEvents = true;

        public bool sendNavigationEvents
        {
            get { return m_sendNavigationEvents; }
            set { m_sendNavigationEvents = value; }
        }

        [SerializeField]
        private int m_DragThreshold = 5;
        /// <summary>
        /// ������ֵ
        /// </summary>
        public int pixelDragThreshold
        {
            get { return m_DragThreshold; }
            set { m_DragThreshold = value; }
        }

        private GameObject m_CurrentSelected;
        /// <summary>
        /// ��ǰ������ģʽ
        /// </summary>
        public BaseInputModule currentInputModule
        {
            get { return m_CurrentInputModule; }
        }

        /// <summary>
        /// Only one object can be selected at a time. Think: controller-selected button.
        /// ͬһʱ��ֻҪһ��������Ա�ѡ��
        /// </summary>
        public GameObject firstSelectedGameObject
        {
            get { return m_FirstSelected; }
            set { m_FirstSelected = value; }
        }

        /// <summary>
        /// ��ǰѡ�������
        /// </summary>
        public GameObject currentSelectedGameObject
        {
            get { return m_CurrentSelected; }
        }

        /// <summary>
        /// ���һ��ѡ������壬�ѱ�����
        /// </summary>
        [Obsolete("lastSelectedGameObject is no longer supported")]
        public GameObject lastSelectedGameObject
        {
            get { return null; }
        }

        private bool m_HasFocus = true;

        /// <summary>
        /// �Ƿ��н���
        /// </summary>
        public bool isFocused
        {
            get { return m_HasFocus; }
        }

        protected EventSystem()
        {}

        /// <summary>
        /// ��������ϵͳ��ɾ����û�л������ϵͳ
        /// </summary>
        public void UpdateModules()
        {
            GetComponents(m_SystemInputModules);
            for (int i = m_SystemInputModules.Count - 1; i >= 0; i--)
            {
                if (m_SystemInputModules[i] && m_SystemInputModules[i].IsActive())
                    continue;
                //ɾ����û�л������ϵͳ
                m_SystemInputModules.RemoveAt(i);
            }
        }

        private bool m_SelectionGuard;
        /// <summary>
        /// ѡ�񱣻�
        /// </summary>
        public bool alreadySelecting
        {
            get { return m_SelectionGuard; }
        }

        /// <summary>
        /// ����ѡ�����
        /// </summary>
        /// <param name="selected">ѡ��Ķ���</param>
        /// <param name="pointer">�����¼�����</param>
        public void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
        {
            if (m_SelectionGuard)
            {
                //����Ѿ�ѡ��һ�����壬����ѡ������������
                Debug.LogError("Attempting to select " + selected +  "while already selecting an object.");
                return;
            }
            //���ѡ�������ʱ��ǰ�������򷵻�
            m_SelectionGuard = true;
            if (selected == m_CurrentSelected)
            {
                m_SelectionGuard = false;
                return;
            }
            //��ǰѡ�������ִ��δѡ���¼�
            // Debug.Log("Selection: new (" + selected + ") old (" + m_CurrentSelected + ")");
            ExecuteEvents.Execute(m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
            m_CurrentSelected = selected;
            //��ѡ�������ִ��ѡ���¼�
            ExecuteEvents.Execute(m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
            m_SelectionGuard = false;
        }

        private BaseEventData m_DummyData;
        /// <summary>
        /// �����¼����ݻ���
        /// </summary>
        private BaseEventData baseEventDataCache
        {
            get
            {
                if (m_DummyData == null)
                    m_DummyData = new BaseEventData(this);

                return m_DummyData;
            }
        }
        /// <summary>
        /// ����ѡ��Ķ���
        /// </summary>
        /// <param name="selected">ѡ��Ķ���</param>
        public void SetSelectedGameObject(GameObject selected)
        {
            SetSelectedGameObject(selected, baseEventDataCache);
        }

        /// <summary>
        /// ���߽���Ƚϣ��������߽��������
        /// </summary>
        /// <param name="lhs">���߽��</param>
        /// <param name="rhs">���߽��</param>
        /// <returns></returns>
        private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
        {
            if (lhs.module != rhs.module)
            {
                var lhsEventCamera = lhs.module.eventCamera;
                var rhsEventCamera = rhs.module.eventCamera;
                //���Ȱ��¼��������������������ԽС��Խ�����ţ�
                if (lhsEventCamera != null && rhsEventCamera != null && lhsEventCamera.depth != rhsEventCamera.depth)
                {
                    // need to reverse the standard compareTo
                    if (lhsEventCamera.depth < rhsEventCamera.depth)
                        return 1;
                    if (lhsEventCamera.depth == rhsEventCamera.depth)
                        return 0;

                    return -1;
                }

                //��sortOrderPriority����
                if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
                    return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
                //����Ⱦ����
                if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
                    return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
            }

            //����Ⱦ�㼶
            if (lhs.sortingLayer != rhs.sortingLayer)
            {
                // Uses the layer value to properly compare the relative order of the layers.
                var rid = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
                var lid = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
                return rid.CompareTo(lid);
            }

            //����Ⱦ����
            if (lhs.sortingOrder != rhs.sortingOrder)
                return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
            //�����
            if (lhs.depth != rhs.depth)
                return rhs.depth.CompareTo(lhs.depth);
            //������
            if (lhs.distance != rhs.distance)
                return lhs.distance.CompareTo(rhs.distance);
            //���±�
            return lhs.index.CompareTo(rhs.index);
        }

        private static readonly Comparison<RaycastResult> s_RaycastComparer = RaycastComparer;
        //���߼������
        public void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
        {
            //���Ͷ����
            raycastResults.Clear();
            var modules = RaycasterManager.GetRaycasters();
            for (int i = 0; i < modules.Count; ++i)
            {
                var module = modules[i];
                if (module == null || !module.IsActive())
                    continue;
                //������λ��Ͷ�����ߣ����Ͷ����
                module.Raycast(eventData, raycastResults);
            }
            //Ͷ��������
            raycastResults.Sort(s_RaycastComparer);
        }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        /// <returns></returns>
        public bool IsPointerOverGameObject()
        {
            return IsPointerOverGameObject(PointerInputModule.kMouseLeftId);
        }

        public bool IsPointerOverGameObject(int pointerId)
        {
            if (m_CurrentInputModule == null)
                return false;

            return m_CurrentInputModule.IsPointerOverGameObject(pointerId);
        }

        /// <summary>
        /// �����¼��������¼�ϵͳ�б��������
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            m_EventSystems.Add(this);
        }

        /// <summary>
        /// �����¼��������ͷ�����ϵͳ���¼�ϵͳ�б��Ƴ�����
        /// </summary>
        protected override void OnDisable()
        {
            if (m_CurrentInputModule != null)
            {
                m_CurrentInputModule.DeactivateModule();
                m_CurrentInputModule = null;
            }

            m_EventSystems.Remove(this);

            base.OnDisable();
        }
        /// <summary>
        /// ����ģ�����λ��
        /// </summary>
        private void TickModules()
        {
            for (var i = 0; i < m_SystemInputModules.Count; i++)
            {
                if (m_SystemInputModules[i] != null)
                    m_SystemInputModules[i].UpdateModule();
            }
        }

        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            m_HasFocus = hasFocus;
        }
        /// <summary>
        /// �����¼�
        /// </summary>
        protected virtual void Update()
        {
            if (current != this)
                return;
            //����ģ�����λ��
            TickModules();

            bool changedModule = false;
            //��������ϵͳ
            for (var i = 0; i < m_SystemInputModules.Count; i++)
            {
                var module = m_SystemInputModules[i];
                if (module.IsModuleSupported() && module.ShouldActivateModule())
                {
                    if (m_CurrentInputModule != module)
                    {
                        ChangeEventModule(module);
                        changedModule = true;
                    }
                    break;
                }
            }

            // no event module set... set the first valid one...
            if (m_CurrentInputModule == null)
            {
                for (var i = 0; i < m_SystemInputModules.Count; i++)
                {
                    var module = m_SystemInputModules[i];
                    if (module.IsModuleSupported())
                    {
                        ChangeEventModule(module);
                        changedModule = true;
                        break;
                    }
                }
            }
            //û�и��¹�ģ����ģ�鲻Ϊnullִ��
            if (!changedModule && m_CurrentInputModule != null)
                m_CurrentInputModule.Process();
        }
        /// <summary>
        /// �ı��¼�����ϵͳ
        /// </summary>
        /// <param name="module"></param>
        private void ChangeEventModule(BaseInputModule module)
        {
            if (m_CurrentInputModule == module)
                return;
            //֮ǰ�¼�����ϵͳ�ر�
            if (m_CurrentInputModule != null)
                m_CurrentInputModule.DeactivateModule();
            //��ǰ�¼�ϵͳ����
            if (module != null)
                module.ActivateModule();
            m_CurrentInputModule = module;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<b>Selected:</b>" + currentSelectedGameObject);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(m_CurrentInputModule != null ? m_CurrentInputModule.ToString() : "No module");
            return sb.ToString();
        }
    }
}
