namespace UnityEngine.EventSystems
{
    /// <summary>
    /// ��������
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
        /// �Ƿ�������ϵͳ
        /// </summary>
        public virtual bool mousePresent
        {
            get { return Input.mousePresent; }
        }
        /// <summary>
        /// ��갴������
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public virtual bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }
        /// <summary>
        /// ��갴����
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public virtual bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public virtual bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }
        /// <summary>
        /// ��굱ǰ����������
        /// </summary>
        public virtual Vector2 mousePosition
        {
            get { return Input.mousePosition; }
        }
        /// <summary>
        /// ������ƫ�ƣ�ֻ��y�����ã�
        /// </summary>
        public virtual Vector2 mouseScrollDelta
        {
            get { return Input.mouseScrollDelta; }
        }
        /// <summary>
        /// �豸�Ƿ�֧�ִ�������
        /// </summary>
        public virtual bool touchSupported
        {
            get { return Input.touchSupported; }
        }
        /// <summary>
        /// ����������
        /// </summary>
        public virtual int touchCount
        {
            get { return Input.touchCount; }
        }

        /// <summary>
        /// �����±��ô���
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
        /// �Ƿ񰴼�����
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public virtual bool GetButtonDown(string buttonName)
        {
            return Input.GetButtonDown(buttonName);
        }
    }
}
