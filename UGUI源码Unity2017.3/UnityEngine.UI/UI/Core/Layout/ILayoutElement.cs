using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    /// <summary>
    /// ����Ԫ��
    /// </summary>
    public interface ILayoutElement
    {
        // After this method is invoked, layout horizontal input properties should return up-to-date values.
        // Children will already have up-to-date layout horizontal inputs when this methods is called.
        /// <summary>
        /// ���㲼�����ˮƽֵ
        /// </summary>
        void CalculateLayoutInputHorizontal();
        // After this method is invoked, layout vertical input properties should return up-to-date values.
        // Children will already have up-to-date layout vertical inputs when this methods is called.
        /// <summary>
        /// ���㲼�������ֱֵ
        /// </summary>
        void CalculateLayoutInputVertical();

        // Layout horizontal inputs
        /// <summary>
        /// ��С���
        /// </summary>
        float minWidth { get; }
        /// <summary>
        /// ���ſ��
        /// </summary>
        float preferredWidth { get; }
        /// <summary>
        /// ���Կ��
        /// </summary>
        float flexibleWidth { get; }
        // Layout vertical inputs
        /// <summary>
        /// ��С�߶�
        /// </summary>
        float minHeight { get; }
        /// <summary>
        /// ���Ÿ߶�
        /// </summary>
        float preferredHeight { get; }
        /// <summary>
        /// ���Կ��
        /// </summary>
        float flexibleHeight { get; }
        /// <summary>
        /// �������ȼ�
        /// </summary>
        int layoutPriority { get; }
    }

    /// <summary>
    /// ���ֿ���
    /// </summary>
    public interface ILayoutController
    {
        /// <summary>
        /// ���ò���ˮƽֵ
        /// </summary>
        void SetLayoutHorizontal();
        /// <summary>
        /// ���ò��ִ�ֱֵ
        /// </summary>
        void SetLayoutVertical();
    }

    /// <summary>
    /// ������
    /// </summary>
    // An ILayoutGroup component should drive the RectTransforms of its children.
    public interface ILayoutGroup : ILayoutController
    {
    }

    /// <summary>
    /// �����Կ���
    /// </summary>
    // An ILayoutSelfController component should drive its own RectTransform.
    public interface ILayoutSelfController : ILayoutController
    {
    }

    /// <summary>
    /// ��������
    /// </summary>
    // An ILayoutIgnorer component is ignored by the auto-layout system.
    public interface ILayoutIgnorer
    {
        bool ignoreLayout { get; }
    }
}
