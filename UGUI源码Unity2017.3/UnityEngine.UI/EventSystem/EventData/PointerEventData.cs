using System;
using System.Text;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    /// <summary>
    /// Each touch event creates one of these containing all the relevant information.
    /// 每个触摸事件会创建一个PointerEventData包含所有相关的信息
    /// </summary>
    public class PointerEventData : BaseEventData
    {
        /// <summary>
        /// 按键枚举，左右中间
        /// </summary>
        public enum InputButton
        {
            Left = 0,
            Right = 1,
            Middle = 2
        }

        /// <summary>
        /// 按键过程枚举
        /// </summary>
        public enum FramePressState
        {
            Pressed,                //按压
            Released,               //释放
            PressedAndReleased,     //按压并释放
            NotChanged              //没有改变
        }

        /// <summary>
        /// 按键进入
        /// </summary>
        public GameObject pointerEnter { get; set; }

        // The object that received OnPointerDown
        /// <summary>
        /// 接受按压的对象
        /// </summary>
        private GameObject m_PointerPress;
        // The object last received OnPointerDown
        public GameObject lastPress { get; private set; }
        // The object that the press happened on even if it can not handle the press event
        public GameObject rawPointerPress { get; set; }
        // The object that received OnDrag
        public GameObject pointerDrag { get; set; }
        
        /// <summary>
        /// 当前投射的第一个结果
        /// </summary>
        public RaycastResult pointerCurrentRaycast { get; set; }
        public RaycastResult pointerPressRaycast { get; set; }

        public List<GameObject> hovered = new List<GameObject>();
        /// <summary>
        /// 是否有资格点击
        /// </summary>
        public bool eligibleForClick { get; set; }

        public int pointerId { get; set; }

        // Current position of the mouse or touch event
        /// <summary>
        /// 当前触碰的位置
        /// </summary>
        public Vector2 position { get; set; }
        // Delta since last update
        public Vector2 delta { get; set; }
        // Position of the press event
        public Vector2 pressPosition { get; set; }
        // World-space position where a ray cast into the screen hits something
        [Obsolete("Use either pointerCurrentRaycast.worldPosition or pointerPressRaycast.worldPosition")]
        public Vector3 worldPosition { get; set; }
        // World-space normal where a ray cast into the screen hits something
        [Obsolete("Use either pointerCurrentRaycast.worldNormal or pointerPressRaycast.worldNormal")]
        public Vector3 worldNormal { get; set; }
        // The last time a click event was sent out (used for double-clicks)
        public float clickTime { get; set; }
        // Number of clicks in a row. 2 for a double-click for example.
        public int clickCount { get; set; }

        public Vector2 scrollDelta { get; set; }
        public bool useDragThreshold { get; set; }
        public bool dragging { get; set; }

        public InputButton button { get; set; }

        public PointerEventData(EventSystem eventSystem) : base(eventSystem)
        {
            eligibleForClick = false;

            pointerId = -1;
            position = Vector2.zero; // Current position of the mouse or touch event
            delta = Vector2.zero; // Delta since last update
            pressPosition = Vector2.zero; // Delta since the event started being tracked
            clickTime = 0.0f; // The last time a click event was sent out (used for double-clicks)
            clickCount = 0; // Number of clicks in a row. 2 for a double-click for example.

            scrollDelta = Vector2.zero;
            useDragThreshold = true;
            dragging = false;
            button = InputButton.Left;
        }
        /// <summary>
        /// 是否移动（增量大于0就代表可以移动）
        /// </summary>
        /// <returns></returns>
        public bool IsPointerMoving()
        {
            return delta.sqrMagnitude > 0.0f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsScrolling()
        {
            return scrollDelta.sqrMagnitude > 0.0f;
        }

        /// <summary>
        /// 当前投射结果模块的事件摄像机（进入事件摄像机）
        /// </summary>
        public Camera enterEventCamera
        {
            get { return pointerCurrentRaycast.module == null ? null : pointerCurrentRaycast.module.eventCamera; }
        }
        /// <summary>
        /// 按压投射结果模块的事件摄像机（按压事件摄像机）
        /// </summary>
        public Camera pressEventCamera
        {
            get { return pointerPressRaycast.module == null ? null : pointerPressRaycast.module.eventCamera; }
        }
        /// <summary>
        /// 按压对象
        /// </summary>
        public GameObject pointerPress
        {
            get { return m_PointerPress; }
            set
            {
                if (m_PointerPress == value)
                    return;

                lastPress = m_PointerPress;
                m_PointerPress = value;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<b>Position</b>: " + position);
            sb.AppendLine("<b>delta</b>: " + delta);
            sb.AppendLine("<b>eligibleForClick</b>: " + eligibleForClick);
            sb.AppendLine("<b>pointerEnter</b>: " + pointerEnter);
            sb.AppendLine("<b>pointerPress</b>: " + pointerPress);
            sb.AppendLine("<b>lastPointerPress</b>: " + lastPress);
            sb.AppendLine("<b>pointerDrag</b>: " + pointerDrag);
            sb.AppendLine("<b>Use Drag Threshold</b>: " + useDragThreshold);
            sb.AppendLine("<b>Current Rayast:</b>");
            sb.AppendLine(pointerCurrentRaycast.ToString());
            sb.AppendLine("<b>Press Rayast:</b>");
            sb.AppendLine(pointerPressRaycast.ToString());
            return sb.ToString();
        }
    }
}
