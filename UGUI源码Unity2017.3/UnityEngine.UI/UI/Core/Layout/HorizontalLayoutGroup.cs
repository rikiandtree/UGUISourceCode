namespace UnityEngine.UI
{
    /// <summary>
    /// 水平自动布局系统
    /// </summary>
    [AddComponentMenu("Layout/Horizontal Layout Group", 150)]
    public class HorizontalLayoutGroup : HorizontalOrVerticalLayoutGroup
    {
        protected HorizontalLayoutGroup()
        {}

        /// <summary>
        /// 计算布局输出水平
        /// </summary>
        public override void CalculateLayoutInputHorizontal()
        {
            //获得子对象
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, false);
        }

        /// <summary>
        /// 计算布局输出垂直
        /// </summary>
        public override void CalculateLayoutInputVertical()
        {
            CalcAlongAxis(1, false);
        }

        /// <summary>
        /// 设置子类水平值
        /// </summary>
        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0, false);
        }

        /// <summary>
        /// 设置子类垂直值
        /// </summary>
        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1, false);
        }
    }
}
