using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    /// <summary>
    /// 布局元素
    /// </summary>
    public interface ILayoutElement
    {
        // After this method is invoked, layout horizontal input properties should return up-to-date values.
        // Children will already have up-to-date layout horizontal inputs when this methods is called.
        /// <summary>
        /// 计算布局输出水平值
        /// </summary>
        void CalculateLayoutInputHorizontal();
        // After this method is invoked, layout vertical input properties should return up-to-date values.
        // Children will already have up-to-date layout vertical inputs when this methods is called.
        /// <summary>
        /// 计算布局输出垂直值
        /// </summary>
        void CalculateLayoutInputVertical();

        // Layout horizontal inputs
        /// <summary>
        /// 最小宽度
        /// </summary>
        float minWidth { get; }
        /// <summary>
        /// 最优宽度
        /// </summary>
        float preferredWidth { get; }
        /// <summary>
        /// 弹性宽度
        /// </summary>
        float flexibleWidth { get; }
        // Layout vertical inputs
        /// <summary>
        /// 最小高度
        /// </summary>
        float minHeight { get; }
        /// <summary>
        /// 最优高度
        /// </summary>
        float preferredHeight { get; }
        /// <summary>
        /// 弹性宽度
        /// </summary>
        float flexibleHeight { get; }
        /// <summary>
        /// 布局优先级
        /// </summary>
        int layoutPriority { get; }
    }

    /// <summary>
    /// 布局控制
    /// </summary>
    public interface ILayoutController
    {
        /// <summary>
        /// 设置布局水平值
        /// </summary>
        void SetLayoutHorizontal();
        /// <summary>
        /// 设置布局垂直值
        /// </summary>
        void SetLayoutVertical();
    }

    /// <summary>
    /// 布局组
    /// </summary>
    // An ILayoutGroup component should drive the RectTransforms of its children.
    public interface ILayoutGroup : ILayoutController
    {
    }

    /// <summary>
    /// 布局自控制
    /// </summary>
    // An ILayoutSelfController component should drive its own RectTransform.
    public interface ILayoutSelfController : ILayoutController
    {
    }

    /// <summary>
    /// 布局屏蔽
    /// </summary>
    // An ILayoutIgnorer component is ignored by the auto-layout system.
    public interface ILayoutIgnorer
    {
        bool ignoreLayout { get; }
    }
}
