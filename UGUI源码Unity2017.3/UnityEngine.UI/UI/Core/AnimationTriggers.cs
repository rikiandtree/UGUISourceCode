using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    /// <summary>
    /// ��������������ѡ��ؼ���ͨ����������ѹ������״̬�Ķ������ѿ�����
    /// </summary>
    [Serializable]
    public class AnimationTriggers
    {
        private const string kDefaultNormalAnimName      = "Normal";
        private const string kDefaultSelectedAnimName = "Highlighted";
        private const string kDefaultPressedAnimName     = "Pressed";
        private const string kDefaultDisabledAnimName    = "Disabled";

        /// <summary>
        /// ��ͨ����
        /// </summary>
        [FormerlySerializedAs("normalTrigger")]
        [SerializeField]
        private string m_NormalTrigger    = kDefaultNormalAnimName;

        /// <summary>
        /// ��������
        /// </summary>
        [FormerlySerializedAs("highlightedTrigger")]
        [FormerlySerializedAs("m_SelectedTrigger")]
        [SerializeField]
        private string m_HighlightedTrigger = kDefaultSelectedAnimName;

        /// <summary>
        /// ��ѹ����
        /// </summary>
        [FormerlySerializedAs("pressedTrigger")]
        [SerializeField]
        private string m_PressedTrigger = kDefaultPressedAnimName;

        /// <summary>
        /// ���ö���
        /// </summary>
        [FormerlySerializedAs("disabledTrigger")]
        [SerializeField]
        private string m_DisabledTrigger = kDefaultDisabledAnimName;

        public string normalTrigger      { get { return m_NormalTrigger; } set { m_NormalTrigger = value; } }
        public string highlightedTrigger { get { return m_HighlightedTrigger; } set { m_HighlightedTrigger = value; } }
        public string pressedTrigger     { get { return m_PressedTrigger; } set { m_PressedTrigger = value; } }
        public string disabledTrigger    { get { return m_DisabledTrigger; } set { m_DisabledTrigger = value; } }
    }
}
