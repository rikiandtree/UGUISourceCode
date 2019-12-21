namespace UnityEngine.EventSystems
{
    /// <summary>
    /// 基础输入
    /// </summary>
    public class BaseInput : UIBehaviour
    {
        public virtual string compositionString
        {
            get { return Input.compositionString; }
        }

        public virtual IMECompositionMode imeCompositionMode
        {
            get { return Input.imeCompositionMode; }
            set { Input.imeCompositionMode = value; }
        }

        public virtual Vector2 compositionCursorPos
        {
            get { return Input.compositionCursorPos; }
            set { Input.compositionCursorPos = value; }
        }

        /// <summary>
        /// 是否有输入系统
        /// </summary>
        public virtual bool mousePresent
        {
            get { return Input.mousePresent; }
        }
        /// <summary>
        /// 鼠标按键按下
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public virtual bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }
        /// <summary>
        /// 鼠标按键起
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public virtual bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }
        /// <summary>
        /// 按键
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public virtual bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }
        /// <summary>
        /// 鼠标当前的像素坐标
        /// </summary>
        public virtual Vector2 mousePosition
        {
            get { return Input.mousePosition; }
        }
        /// <summary>
        /// 鼠标滚轮偏移（只有y轴有用）
        /// </summary>
        public virtual Vector2 mouseScrollDelta
        {
            get { return Input.mouseScrollDelta; }
        }
        /// <summary>
        /// 设备是否支持触摸输入
        /// </summary>
        public virtual bool touchSupported
        {
            get { return Input.touchSupported; }
        }
        /// <summary>
        /// 触摸点数量
        /// </summary>
        public virtual int touchCount
        {
            get { return Input.touchCount; }
        }

        /// <summary>
        /// 根据下标获得触摸
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Touch GetTouch(int index)
        {
            return Input.GetTouch(index);
        }

        public virtual float GetAxisRaw(string axisName)
        {
            return Input.GetAxisRaw(axisName);
        }

        /// <summary>
        /// 是否按键按下
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public virtual bool GetButtonDown(string buttonName)
        {
            return Input.GetButtonDown(buttonName);
        }
    }
}
