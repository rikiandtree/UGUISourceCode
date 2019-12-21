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
        //输入系统
        private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();
        //当前输入系统
        private BaseInputModule m_CurrentInputModule;
        //事件系统列表
        private  static List<EventSystem> m_EventSystems = new List<EventSystem>();
        //当前事件
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
        /// 像素阈值
        /// </summary>
        public int pixelDragThreshold
        {
            get { return m_DragThreshold; }
            set { m_DragThreshold = value; }
        }

        private GameObject m_CurrentSelected;
        /// <summary>
        /// 当前的输入模式
        /// </summary>
        public BaseInputModule currentInputModule
        {
            get { return m_CurrentInputModule; }
        }

        /// <summary>
        /// Only one object can be selected at a time. Think: controller-selected button.
        /// 同一时间只要一个物体可以被选择
        /// </summary>
        public GameObject firstSelectedGameObject
        {
            get { return m_FirstSelected; }
            set { m_FirstSelected = value; }
        }

        /// <summary>
        /// 当前选择的物体
        /// </summary>
        public GameObject currentSelectedGameObject
        {
            get { return m_CurrentSelected; }
        }

        /// <summary>
        /// 最后一个选择的物体，已被废弃
        /// </summary>
        [Obsolete("lastSelectedGameObject is no longer supported")]
        public GameObject lastSelectedGameObject
        {
            get { return null; }
        }

        private bool m_HasFocus = true;

        /// <summary>
        /// 是否有焦点
        /// </summary>
        public bool isFocused
        {
            get { return m_HasFocus; }
        }

        protected EventSystem()
        {}

        /// <summary>
        /// 更新输入系统，删除掉没有活动的输入系统
        /// </summary>
        public void UpdateModules()
        {
            GetComponents(m_SystemInputModules);
            for (int i = m_SystemInputModules.Count - 1; i >= 0; i--)
            {
                if (m_SystemInputModules[i] && m_SystemInputModules[i].IsActive())
                    continue;
                //删除掉没有活动的输入系统
                m_SystemInputModules.RemoveAt(i);
            }
        }

        private bool m_SelectionGuard;
        /// <summary>
        /// 选择保护
        /// </summary>
        public bool alreadySelecting
        {
            get { return m_SelectionGuard; }
        }

        /// <summary>
        /// 设置选择对象
        /// </summary>
        /// <param name="selected">选择的对象</param>
        /// <param name="pointer">基础事件数据</param>
        public void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
        {
            if (m_SelectionGuard)
            {
                //如果已经选择一个物体，则不能选择其他的物体
                Debug.LogError("Attempting to select " + selected +  "while already selecting an object.");
                return;
            }
            //如果选择的物体时当前的物体则返回
            m_SelectionGuard = true;
            if (selected == m_CurrentSelected)
            {
                m_SelectionGuard = false;
                return;
            }
            //当前选择的物体执行未选择事件
            // Debug.Log("Selection: new (" + selected + ") old (" + m_CurrentSelected + ")");
            ExecuteEvents.Execute(m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
            m_CurrentSelected = selected;
            //新选择的物体执行选择事件
            ExecuteEvents.Execute(m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
            m_SelectionGuard = false;
        }

        private BaseEventData m_DummyData;
        /// <summary>
        /// 基础事件数据缓存
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
        /// 设置选择的对象
        /// </summary>
        /// <param name="selected">选择的对象</param>
        public void SetSelectedGameObject(GameObject selected)
        {
            SetSelectedGameObject(selected, baseEventDataCache);
        }

        /// <summary>
        /// 射线结果比较（用于射线结果的排序）
        /// </summary>
        /// <param name="lhs">射线结果</param>
        /// <param name="rhs">射线结果</param>
        /// <returns></returns>
        private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
        {
            if (lhs.module != rhs.module)
            {
                var lhsEventCamera = lhs.module.eventCamera;
                var rhsEventCamera = rhs.module.eventCamera;
                //首先按事件的摄像机的深度排序（深度越小，越往后排）
                if (lhsEventCamera != null && rhsEventCamera != null && lhsEventCamera.depth != rhsEventCamera.depth)
                {
                    // need to reverse the standard compareTo
                    if (lhsEventCamera.depth < rhsEventCamera.depth)
                        return 1;
                    if (lhsEventCamera.depth == rhsEventCamera.depth)
                        return 0;

                    return -1;
                }

                //按sortOrderPriority排序
                if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
                    return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
                //按渲染级别
                if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
                    return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
            }

            //按渲染层级
            if (lhs.sortingLayer != rhs.sortingLayer)
            {
                // Uses the layer value to properly compare the relative order of the layers.
                var rid = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
                var lid = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
                return rid.CompareTo(lid);
            }

            //按渲染序列
            if (lhs.sortingOrder != rhs.sortingOrder)
                return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
            //按深度
            if (lhs.depth != rhs.depth)
                return rhs.depth.CompareTo(lhs.depth);
            //按距离
            if (lhs.distance != rhs.distance)
                return lhs.distance.CompareTo(rhs.distance);
            //按下标
            return lhs.index.CompareTo(rhs.index);
        }

        private static readonly Comparison<RaycastResult> s_RaycastComparer = RaycastComparer;
        //射线检测所有
        public void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
        {
            //清空投射结果
            raycastResults.Clear();
            var modules = RaycasterManager.GetRaycasters();
            for (int i = 0; i < modules.Count; ++i)
            {
                var module = modules[i];
                if (module == null || !module.IsActive())
                    continue;
                //向输入位置投射射线，获得投射结果
                module.Raycast(eventData, raycastResults);
            }
            //投射结果排序
            raycastResults.Sort(s_RaycastComparer);
        }
        /// <summary>
        /// 是否是
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
        /// 可用事件函数：事件系统列表添加自身
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            m_EventSystems.Add(this);
        }

        /// <summary>
        /// 禁用事件函数：释放输入系统，事件系统列表移除自身
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
        /// 更新模块鼠标位置
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
        /// 更新事件
        /// </summary>
        protected virtual void Update()
        {
            if (current != this)
                return;
            //更新模块鼠标位置
            TickModules();

            bool changedModule = false;
            //遍历输入系统
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
            //没有更新过模块且模块不为null执行
            if (!changedModule && m_CurrentInputModule != null)
                m_CurrentInputModule.Process();
        }
        /// <summary>
        /// 改变事件处理系统
        /// </summary>
        /// <param name="module"></param>
        private void ChangeEventModule(BaseInputModule module)
        {
            if (m_CurrentInputModule == module)
                return;
            //之前事件处理系统关闭
            if (m_CurrentInputModule != null)
                m_CurrentInputModule.DeactivateModule();
            //当前事件系统激活
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
