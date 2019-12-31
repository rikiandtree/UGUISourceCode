namespace UnityEngine.UI
{
    /// <summary>
    /// ˮƽ�Զ�����ϵͳ
    /// </summary>
    [AddComponentMenu("Layout/Horizontal Layout Group", 150)]
    public class HorizontalLayoutGroup : HorizontalOrVerticalLayoutGroup
    {
        protected HorizontalLayoutGroup()
        {}

        /// <summary>
        /// ���㲼�����ˮƽ
        /// </summary>
        public override void CalculateLayoutInputHorizontal()
        {
            //����Ӷ���
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, false);
        }

        /// <summary>
        /// ���㲼�������ֱ
        /// </summary>
        public override void CalculateLayoutInputVertical()
        {
            CalcAlongAxis(1, false);
        }

        /// <summary>
        /// ��������ˮƽֵ
        /// </summary>
        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0, false);
        }

        /// <summary>
        /// �������ഹֱֵ
        /// </summary>
        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1, false);
        }
    }
}
