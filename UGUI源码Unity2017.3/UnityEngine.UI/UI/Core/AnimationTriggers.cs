using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    /// <summary>
    /// 动画触发，用于选择控件普通，经过，按压，禁用状态的动画（已看过）
    /// </summary>
    [Serializable]
    public class AnimationTriggers
    {
        private const string kDefaultNormalAnimName      = "Normal";
        private const string kDefaultSelectedAnimName = "Highlighted";
        private const string kDefaultPressedAnimName     = "Pressed";
        private const string kDefaultDisabledAnimName    = "Disabled";

        /// <summary>
        /// 普通动画
        /// </summary>
        [FormerlySerializedAs("normalTrigger")]
        [SerializeField]
        private string m_NormalTrigger    = kDefaultNormalAnimName;

        /// <summary>
        /// 经过动画
        /// </summary>
        [FormerlySerializedAs("highlightedTrigger")]
        [FormerlySerializedAs("m_SelectedTrigger")]
        [SerializeField]
        private string m_HighlightedTrigger = kDefaultSelectedAnimName;

        /// <summary>
        /// 按压动画
        /// </summary>
        [FormerlySerializedAs("pressedTrigger")]
        [SerializeField]
        private string m_PressedTrigger = kDefaultPressedAnimName;

        /// <summary>
        /// 禁用动画
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
