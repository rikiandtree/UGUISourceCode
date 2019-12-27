using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 单选框组（已看过）
    /// </summary>
    [AddComponentMenu("UI/Toggle Group", 32)]
    [DisallowMultipleComponent]
    public class ToggleGroup : UIBehaviour
    {
        /// <summary>
        /// 是否允许所有选框关闭
        /// </summary>
        [SerializeField] private bool m_AllowSwitchOff = false;
        public bool allowSwitchOff { get { return m_AllowSwitchOff; } set { m_AllowSwitchOff = value; } }

        /// <summary>
        /// 单选框组下的单选框
        /// </summary>
        private List<Toggle> m_Toggles = new List<Toggle>();

        protected ToggleGroup()
        {}

        /// <summary>
        /// 单选框是否在组内
        /// </summary>
        /// <param name="toggle"></param>
        private void ValidateToggleIsInGroup(Toggle toggle)
        {
            if (toggle == null || !m_Toggles.Contains(toggle))
                throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[] {toggle, this}));
        }

        /// <summary>
        /// 发布单选框被点击
        /// </summary>
        /// <param name="toggle"></param>
        public void NotifyToggleOn(Toggle toggle)
        {
            ValidateToggleIsInGroup(toggle);

            // disable all toggles in the group
            for (var i = 0; i < m_Toggles.Count; i++)
            {
                if (m_Toggles[i] == toggle)
                    continue;

                m_Toggles[i].isOn = false;
            }
        }

        /// <summary>
        /// 移除单选框
        /// </summary>
        /// <param name="toggle"></param>
        public void UnregisterToggle(Toggle toggle)
        {
            if (m_Toggles.Contains(toggle))
                m_Toggles.Remove(toggle);
        }

        /// <summary>
        /// 注册单选框
        /// </summary>
        /// <param name="toggle"></param>
        public void RegisterToggle(Toggle toggle)
        {
            if (!m_Toggles.Contains(toggle))
                m_Toggles.Add(toggle);
        }

        /// <summary>
        /// 是否有单选框处于选中状态
        /// </summary>
        /// <returns></returns>
        public bool AnyTogglesOn()
        {
            return m_Toggles.Find(x => x.isOn) != null;
        }

        public IEnumerable<Toggle> ActiveToggles()
        {
            return m_Toggles.Where(x => x.isOn);
        }

        /// <summary>
        /// 设置所有的单选框关闭
        /// </summary>
        public void SetAllTogglesOff()
        {
            bool oldAllowSwitchOff = m_AllowSwitchOff;
            m_AllowSwitchOff = true;

            for (var i = 0; i < m_Toggles.Count; i++)
                m_Toggles[i].isOn = false;

            m_AllowSwitchOff = oldAllowSwitchOff;
        }
    }
}
