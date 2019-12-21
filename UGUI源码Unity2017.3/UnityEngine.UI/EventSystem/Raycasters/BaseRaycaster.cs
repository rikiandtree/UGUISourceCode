using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    /// <summary>
    /// 基础射线投射
    /// </summary>
    public abstract class BaseRaycaster : UIBehaviour
    {
        /// <summary>
        /// 射线投射
        /// </summary>
        /// <param name="eventData">触摸事件</param>
        /// <param name="resultAppendList">射线检测结果</param>
        public abstract void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList);
        /// <summary>
        /// 事件摄像机
        /// </summary>
        public abstract Camera eventCamera { get; }

        [Obsolete("Please use sortOrderPriority and renderOrderPriority", false)]
        public virtual int priority
        {
            get { return 0; }
        }
        /// <summary>
        /// 排序顺序优先级
        /// </summary>
        public virtual int sortOrderPriority
        {
            get { return int.MinValue; }
        }
        /// <summary>
        /// 渲染顺序优先级
        /// </summary>
        public virtual int renderOrderPriority
        {
            get { return int.MinValue; }
        }

        public override string ToString()
        {
            return "Name: " + gameObject + "\n" +
                "eventCamera: " + eventCamera + "\n" +
                "sortOrderPriority: " + sortOrderPriority + "\n" +
                "renderOrderPriority: " + renderOrderPriority;
        }

        /// <summary>
        /// 可用事件函数：将投射者添加进投射者管理类
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            RaycasterManager.AddRaycaster(this);
        }

        /// <summary>
        /// 禁用函数：将投射者移除投射者管理类
        /// </summary>
        protected override void OnDisable()
        {
            RaycasterManager.RemoveRaycasters(this);
            base.OnDisable();
        }
    }
}
