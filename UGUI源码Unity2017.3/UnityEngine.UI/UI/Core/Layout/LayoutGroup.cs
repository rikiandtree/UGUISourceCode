using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public abstract class LayoutGroup : UIBehaviour, ILayoutElement, ILayoutGroup
    {
        /// <summary>
        /// 边距
        /// </summary>
        [SerializeField] protected RectOffset m_Padding = new RectOffset();
        public RectOffset padding { get { return m_Padding; } set { SetProperty(ref m_Padding, value); } }

        /// <summary>
        /// 子对象的排列顺序
        /// </summary>
        [FormerlySerializedAs("m_Alignment")]
        [SerializeField] protected TextAnchor m_ChildAlignment = TextAnchor.UpperLeft;
        public TextAnchor childAlignment { get { return m_ChildAlignment; } set { SetProperty(ref m_ChildAlignment, value); } }

        /// <summary>
        /// transform组件
        /// </summary>
        [System.NonSerialized] private RectTransform m_Rect;
        protected RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        /// <summary>
        /// 禁用属性对象
        /// </summary>
        protected DrivenRectTransformTracker m_Tracker;
        /// <summary>
        /// 总体最小值
        /// </summary>
        private Vector2 m_TotalMinSize = Vector2.zero;
        /// <summary>
        /// 总体最优值
        /// </summary>
        private Vector2 m_TotalPreferredSize = Vector2.zero;
        /// <summary>
        /// 总体弹性值
        /// </summary>
        private Vector2 m_TotalFlexibleSize = Vector2.zero;

        /// <summary>
        /// 子对象transform组件
        /// </summary>
        [System.NonSerialized] private List<RectTransform> m_RectChildren = new List<RectTransform>();
        protected List<RectTransform> rectChildren { get { return m_RectChildren; } }

        // ILayoutElement Interface
        /// <summary>
        /// 计算布局输出水平值
        /// </summary>
        public virtual void CalculateLayoutInputHorizontal()
        {
            m_RectChildren.Clear();
            var toIgnoreList = ListPool<Component>.Get();
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                var rect = rectTransform.GetChild(i) as RectTransform;
                if (rect == null || !rect.gameObject.activeInHierarchy)
                    continue;

                rect.GetComponents(typeof(ILayoutIgnorer), toIgnoreList);

                if (toIgnoreList.Count == 0)
                {
                    m_RectChildren.Add(rect);
                    continue;
                }

                for (int j = 0; j < toIgnoreList.Count; j++)
                {
                    var ignorer = (ILayoutIgnorer)toIgnoreList[j];
                    if (!ignorer.ignoreLayout)
                    {
                        m_RectChildren.Add(rect);
                        break;
                    }
                }
            }
            ListPool<Component>.Release(toIgnoreList);
            m_Tracker.Clear();
        }

        public abstract void CalculateLayoutInputVertical();
        public virtual float minWidth { get { return GetTotalMinSize(0); } }
        public virtual float preferredWidth { get { return GetTotalPreferredSize(0); } }
        public virtual float flexibleWidth { get { return GetTotalFlexibleSize(0); } }
        public virtual float minHeight { get { return GetTotalMinSize(1); } }
        public virtual float preferredHeight { get { return GetTotalPreferredSize(1); } }
        public virtual float flexibleHeight { get { return GetTotalFlexibleSize(1); } }
        /// <summary>
        /// 布局优先级
        /// </summary>
        public virtual int layoutPriority { get { return 0; } }

        // ILayoutController Interface

        public abstract void SetLayoutHorizontal();
        public abstract void SetLayoutVertical();

        // Implementation

        protected LayoutGroup()
        {
            if (m_Padding == null)
                m_Padding = new RectOffset();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            SetDirty();
        }

        /// <summary>
        /// 根据轴获取最小尺寸
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected float GetTotalMinSize(int axis)
        {
            return m_TotalMinSize[axis];
        }

        /// <summary>
        /// 根据轴获取最优尺寸
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected float GetTotalPreferredSize(int axis)
        {
            return m_TotalPreferredSize[axis];
        }

        /// <summary>
        /// 根据轴获取弹性尺寸
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected float GetTotalFlexibleSize(int axis)
        {
            return m_TotalFlexibleSize[axis];
        }

        protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
        {
            float requiredSpace = requiredSpaceWithoutPadding + (axis == 0 ? padding.horizontal : padding.vertical);
            float availableSpace = rectTransform.rect.size[axis];
            float surplusSpace = availableSpace - requiredSpace;
            float alignmentOnAxis = GetAlignmentOnAxis(axis);
            return (axis == 0 ? padding.left : padding.top) + surplusSpace * alignmentOnAxis;
        }

        protected float GetAlignmentOnAxis(int axis)
        {
            if (axis == 0)
                return ((int)childAlignment % 3) * 0.5f;
            else
                return ((int)childAlignment / 3) * 0.5f;
        }

        /// <summary>
        /// 设置轴上的各种属性
        /// </summary>
        /// <param name="totalMin">最小值</param>
        /// <param name="totalPreferred">最优值</param>
        /// <param name="totalFlexible">弹性值</param>
        /// <param name="axis">轴</param>
        protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
        {
            m_TotalMinSize[axis] = totalMin;
            m_TotalPreferredSize[axis] = totalPreferred;
            m_TotalFlexibleSize[axis] = totalFlexible;
        }

        /// <summary>
        /// 设置子对象的沿轴
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        protected void SetChildAlongAxis(RectTransform rect, int axis, float pos)
        {
            if (rect == null)
                return;

            m_Tracker.Add(this, rect,
                DrivenTransformProperties.Anchors |
                (axis == 0 ? DrivenTransformProperties.AnchoredPositionX : DrivenTransformProperties.AnchoredPositionY));

            rect.SetInsetAndSizeFromParentEdge(axis == 0 ? RectTransform.Edge.Left : RectTransform.Edge.Top, pos, rect.sizeDelta[axis]);
        }

        /// <summary>
        /// 设置子对象位置和尺寸
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="axis"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        protected void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
        {
            if (rect == null)
                return;

            m_Tracker.Add(this, rect,
                DrivenTransformProperties.Anchors |
                (axis == 0 ?
                 (DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.SizeDeltaX) :
                 (DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.SizeDeltaY)
                ));

            rect.SetInsetAndSizeFromParentEdge(axis == 0 ? RectTransform.Edge.Left : RectTransform.Edge.Top, pos, size);
        }

        /// <summary>
        /// 是否为根布局组
        /// </summary>
        private bool isRootLayoutGroup
        {
            get
            {
                Transform parent = transform.parent;
                if (parent == null)
                    return true;
                return transform.parent.GetComponent(typeof(ILayoutGroup)) == null;
            }
        }

        /// <summary>
        /// transform组件尺寸改变
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (isRootLayoutGroup)
                SetDirty();
        }

        protected virtual void OnTransformChildrenChanged()
        {
            SetDirty();
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <typeparam name="T">结构体</typeparam>
        /// <param name="currentValue">当前值</param>
        /// <param name="newValue">新值</param>
        protected void SetProperty<T>(ref T currentValue, T newValue)
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return;
            currentValue = newValue;
            SetDirty();
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            if (!CanvasUpdateRegistry.IsRebuildingLayout())
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            else
                StartCoroutine(DelayedSetDirty(rectTransform));
        }

        /// <summary>
        /// 延迟一帧设置数据
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

    #if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

    #endif
    }
}
