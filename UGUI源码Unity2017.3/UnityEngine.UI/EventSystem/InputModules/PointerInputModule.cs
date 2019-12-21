using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
    /// <summary>
    /// ָ������ģ��
    /// </summary>
    public abstract class PointerInputModule : BaseInputModule
    {
        /// <summary>
        /// ������id
        /// </summary>
        public const int kMouseLeftId = -1;
        public const int kMouseRightId = -2;
        public const int kMouseMiddleId = -3;

        public const int kFakeTouchesId = -4;

        protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData>();
        /// <summary>
        /// ��������id���ָ���¼�����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="create"></param>
        /// <returns>�´�����Ϊtrue��ԭ�е�Ϊfalse</returns>
        protected bool GetPointerData(int id, out PointerEventData data, bool create)
        {
            if (!m_PointerData.TryGetValue(id, out data) && create)
            {

                data = new PointerEventData(eventSystem)
                {
                    pointerId = id,
                };
                m_PointerData.Add(id, data);
                return true;
            }
            return false;
        }

        protected void RemovePointerData(PointerEventData data)
        {
            m_PointerData.Remove(data.pointerId);
        }
        /// <summary>
        /// ����Touch���ָ���¼�����
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pressed"></param>
        /// <param name="released"></param>
        /// <returns></returns>
        protected PointerEventData GetTouchPointerEventData(Touch input, out bool pressed, out bool released)
        {
            PointerEventData pointerData;
            
            var created = GetPointerData(input.fingerId, out pointerData, true);
            //����Ϊ����û���ù�
            pointerData.Reset();

            //��ѹ���´������߰�ѹ�׶γ�
            pressed = created || (input.phase == TouchPhase.Began);
            //�ͷ���ȡ�������������ѹ
            released = (input.phase == TouchPhase.Canceled) || (input.phase == TouchPhase.Ended);

            //�´�����pointerdata��λ�þ��������λ��
            if (created)
                pointerData.position = input.position;
            //����ǰ�ѹpointerdata�ķ���������0�����������λ�ü�ȥ��ǰλ��
            if (pressed)
                pointerData.delta = Vector2.zero;
            else
                pointerData.delta = input.position - pointerData.position;
            //��ѹ����λ�ø���Ϊ����λ��
            pointerData.position = input.position;

            //�������
            pointerData.button = PointerEventData.InputButton.Left;

            //����Ͷ��������
            eventSystem.RaycastAll(pointerData, m_RaycastResultCache);
            //��ȡ��һ��Ͷ����
            var raycast = FindFirstRaycast(m_RaycastResultCache);
            pointerData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();
            return pointerData;
        }
        
        protected void CopyFromTo(PointerEventData @from, PointerEventData @to)
        {
            @to.position = @from.position;
            @to.delta = @from.delta;
            @to.scrollDelta = @from.scrollDelta;
            @to.pointerCurrentRaycast = @from.pointerCurrentRaycast;
            @to.pointerEnter = @from.pointerEnter;
        }

        /// <summary>
        /// �����갴���ĵ��״̬
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        protected PointerEventData.FramePressState StateForMouseButton(int buttonId)
        {
            var pressed = input.GetMouseButtonDown(buttonId);
            var released = input.GetMouseButtonUp(buttonId);
            if (pressed && released)
                return PointerEventData.FramePressState.PressedAndReleased;
            if (pressed)
                return PointerEventData.FramePressState.Pressed;
            if (released)
                return PointerEventData.FramePressState.Released;
            return PointerEventData.FramePressState.NotChanged;
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        protected class ButtonState
        {
            private PointerEventData.InputButton m_Button = PointerEventData.InputButton.Left;

            public MouseButtonEventData eventData
            {
                get { return m_EventData; }
                set { m_EventData = value; }
            }

            public PointerEventData.InputButton button
            {
                get { return m_Button; }
                set { m_Button = value; }
            }

            private MouseButtonEventData m_EventData;
        }
        /// <summary>
        /// ���״̬
        /// </summary>
        protected class MouseState
        {
            private List<ButtonState> m_TrackedButtons = new List<ButtonState>();

            /// <summary>
            /// ��һ֡���κΰ�ѹ
            /// </summary>
            /// <returns></returns>
            public bool AnyPressesThisFrame()
            {
                for (int i = 0; i < m_TrackedButtons.Count; i++)
                {
                    if (m_TrackedButtons[i].eventData.PressedThisFrame())
                        return true;
                }
                return false;
            }

            /// <summary>
            /// ��һ֡���κ��ͷ�
            /// </summary>
            /// <returns></returns>
            public bool AnyReleasesThisFrame()
            {
                for (int i = 0; i < m_TrackedButtons.Count; i++)
                {
                    if (m_TrackedButtons[i].eventData.ReleasedThisFrame())
                        return true;
                }
                return false;
            }

            /// <summary>
            /// ��ð���״̬
            /// </summary>
            /// <param name="button"></param>
            /// <returns></returns>
            public ButtonState GetButtonState(PointerEventData.InputButton button)
            {
                ButtonState tracked = null;
                for (int i = 0; i < m_TrackedButtons.Count; i++)
                {
                    if (m_TrackedButtons[i].button == button)
                    {
                        tracked = m_TrackedButtons[i];
                        break;
                    }
                }

                if (tracked == null)
                {
                    tracked = new ButtonState { button = button, eventData = new MouseButtonEventData() };
                    m_TrackedButtons.Add(tracked);
                }
                return tracked;
            }

            /// <summary>
            /// ���ð���״̬
            /// </summary>
            /// <param name="button"></param>
            /// <param name="stateForMouseButton"></param>
            /// <param name="data"></param>
            public void SetButtonState(PointerEventData.InputButton button, PointerEventData.FramePressState stateForMouseButton, PointerEventData data)
            {
                var toModify = GetButtonState(button);
                toModify.eventData.buttonState = stateForMouseButton;
                toModify.eventData.buttonData = data;
            }
        }

        /// <summary>
        /// ��갴���¼�����
        /// </summary>
        public class MouseButtonEventData
        {
            public PointerEventData.FramePressState buttonState;
            public PointerEventData buttonData;

            public bool PressedThisFrame()
            {
                return buttonState == PointerEventData.FramePressState.Pressed || buttonState == PointerEventData.FramePressState.PressedAndReleased;
            }

            public bool ReleasedThisFrame()
            {
                return buttonState == PointerEventData.FramePressState.Released || buttonState == PointerEventData.FramePressState.PressedAndReleased;
            }
        }

        private readonly MouseState m_MouseState = new MouseState();

        protected virtual MouseState GetMousePointerEventData()
        {
            return GetMousePointerEventData(0);
        }

        /// <summary>
        /// �������������¼�����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual MouseState GetMousePointerEventData(int id)
        {
            // Populate the left button...
            //�����������¼���������û��ʹ�ã��Լ����λ��
            PointerEventData leftData;
            var created = GetPointerData(kMouseLeftId, out leftData, true);

            leftData.Reset();

            if (created)
                leftData.position = input.mousePosition;

            Vector2 pos = input.mousePosition;
            
            //����������������״̬�����⴦������λ���Լ�����
            //�������¼�����λ���Լ�����
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // We don't want to do ANY cursor-based interaction when the mouse is locked
                leftData.position = new Vector2(-1.0f, -1.0f);
                leftData.delta = Vector2.zero;
            }
            else
            {
                leftData.delta = pos - leftData.position;
                leftData.position = pos;
            }
            //����������Ͱ����Լ�Ͷ�����
            leftData.scrollDelta = input.mouseScrollDelta;
            leftData.button = PointerEventData.InputButton.Left;
            eventSystem.RaycastAll(leftData, m_RaycastResultCache);
            var raycast = FindFirstRaycast(m_RaycastResultCache);
            leftData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            // copy the apropriate data into right and middle slots
            //�����Ҽ����м���������
            PointerEventData rightData;
            GetPointerData(kMouseRightId, out rightData, true);
            CopyFromTo(leftData, rightData);
            rightData.button = PointerEventData.InputButton.Right;

            PointerEventData middleData;
            GetPointerData(kMouseMiddleId, out middleData, true);
            CopyFromTo(leftData, middleData);
            middleData.button = PointerEventData.InputButton.Middle;

            //�������Ұ�����״̬�����ݱ����������
            m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
            m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
            m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);

            return m_MouseState;
        }

        /// <summary>
        /// ������һ�εĵ���¼�����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected PointerEventData GetLastPointerEventData(int id)
        {
            PointerEventData data;
            GetPointerData(id, out data, false);
            return data;
        }
        /// <summary>
        /// �Ƿ�Ӧ�ÿ�ʼ�϶�
        /// </summary>
        /// <param name="pressPos">��ѹλ��</param>
        /// <param name="currentPos">��ǰλ��</param>
        /// <param name="threshold">��ֵ</param>
        /// <param name="useDragThreshold">�Ƿ�ʹ����ֵ</param>
        /// <returns></returns>
        private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
        {
            if (!useDragThreshold)
                return true;

            return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
        }

        /// <summary>
        /// �����ƶ�
        /// </summary>
        /// <param name="pointerEvent"></param>
        protected virtual void ProcessMove(PointerEventData pointerEvent)
        {
            var targetGO = (Cursor.lockState == CursorLockMode.Locked ? null : pointerEvent.pointerCurrentRaycast.gameObject);
            HandlePointerExitAndEnter(pointerEvent, targetGO);
        }
        /// <summary>
        /// ������ק
        /// </summary>
        /// <param name="pointerEvent"></param>
        protected virtual void ProcessDrag(PointerEventData pointerEvent)
        {
            if (!pointerEvent.IsPointerMoving() ||
                Cursor.lockState == CursorLockMode.Locked ||
                pointerEvent.pointerDrag == null)
                return;

            if (!pointerEvent.dragging
                && ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
            {
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
                pointerEvent.dragging = true;
            }

            // Drag notification
            if (pointerEvent.dragging)
            {
                // Before doing drag we should cancel any pointer down state
                // And clear selection!
                if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                    pointerEvent.eligibleForClick = false;
                    pointerEvent.pointerPress = null;
                    pointerEvent.rawPointerPress = null;
                }
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            }
        }

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        /// <param name="pointerId"></param>
        /// <returns></returns>
        public override bool IsPointerOverGameObject(int pointerId)
        {
            var lastPointer = GetLastPointerEventData(pointerId);
            if (lastPointer != null)
                return lastPointer.pointerEnter != null;
            return false;
        }
        /// <summary>
        /// ���ѡ��
        /// </summary>
        protected void ClearSelection()
        {
            var baseEventData = GetBaseEventData();

            foreach (var pointer in m_PointerData.Values)
            {
                // clear all selection
                HandlePointerExitAndEnter(pointer, null);
            }

            m_PointerData.Clear();
            eventSystem.SetSelectedGameObject(null, baseEventData);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("<b>Pointer Input Module of type: </b>" + GetType());
            sb.AppendLine();
            foreach (var pointer in m_PointerData)
            {
                if (pointer.Value == null)
                    continue;
                sb.AppendLine("<B>Pointer:</b> " + pointer.Key);
                sb.AppendLine(pointer.Value.ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// ѡ��ñ���ѡ����
        /// </summary>
        /// <param name="currentOverGo"></param>
        /// <param name="pointerEvent"></param>
        protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
        {
            // Selection tracking
            var selectHandlerGO = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
            // if we have clicked something new, deselect the old thing
            // leave 'selection handling' up to the press event though.
            if (selectHandlerGO != eventSystem.currentSelectedGameObject)
                eventSystem.SetSelectedGameObject(null, pointerEvent);
        }
    }
}
