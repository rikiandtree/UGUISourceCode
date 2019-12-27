using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    /// <summary>
    /// ѡ��ؼ��ľ���״̬����������ѹ�����ã����ѿ�����
    /// </summary>
    [Serializable]
    public struct SpriteState : IEquatable<SpriteState>
    {
        /// <summary>
        /// ��������
        /// </summary>
        [FormerlySerializedAs("highlightedSprite")]
        [FormerlySerializedAs("m_SelectedSprite")]
        [SerializeField]
        private Sprite m_HighlightedSprite;

        /// <summary>
        /// ��ѹ����
        /// </summary>
        [FormerlySerializedAs("pressedSprite")]
        [SerializeField]
        private Sprite m_PressedSprite;

        /// <summary>
        /// ���þ���
        /// </summary>
        [FormerlySerializedAs("disabledSprite")]
        [SerializeField]
        private Sprite m_DisabledSprite;

        public Sprite highlightedSprite    { get { return m_HighlightedSprite; } set { m_HighlightedSprite = value; } }
        public Sprite pressedSprite     { get { return m_PressedSprite; } set { m_PressedSprite = value; } }
        public Sprite disabledSprite    { get { return m_DisabledSprite; } set { m_DisabledSprite = value; } }

        /// <summary>
        /// ��������״̬�Ƿ����
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
