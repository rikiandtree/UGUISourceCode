using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    /// <summary>
    /// 选择控件的精灵状态（经过，按压，禁用）（已看过）
    /// </summary>
    [Serializable]
    public struct SpriteState : IEquatable<SpriteState>
    {
        /// <summary>
        /// 高亮精灵
        /// </summary>
        [FormerlySerializedAs("highlightedSprite")]
        [FormerlySerializedAs("m_SelectedSprite")]
        [SerializeField]
        private Sprite m_HighlightedSprite;

        /// <summary>
        /// 按压精灵
        /// </summary>
        [FormerlySerializedAs("pressedSprite")]
        [SerializeField]
        private Sprite m_PressedSprite;

        /// <summary>
        /// 禁用精灵
        /// </summary>
        [FormerlySerializedAs("disabledSprite")]
        [SerializeField]
        private Sprite m_DisabledSprite;

        public Sprite highlightedSprite    { get { return m_HighlightedSprite; } set { m_HighlightedSprite = value; } }
        public Sprite pressedSprite     { get { return m_PressedSprite; } set { m_PressedSprite = value; } }
        public Sprite disabledSprite    { get { return m_DisabledSprite; } set { m_DisabledSprite = value; } }

        /// <summary>
        /// 两个精灵状态是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SpriteState other)
        {
            return highlightedSprite == other.highlightedSprite &&
                pressedSprite == other.pressedSprite &&
                disabledSprite == other.disabledSprite;
        }
    }
}
