using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    /// <summary>
    /// ��������Ͷ��
    /// </summary>
    public abstract class BaseRaycaster : UIBehaviour
    {
        /// <summary>
        /// ����Ͷ��
        /// </summary>
        /// <param name="eventData">�����¼�</param>
        /// <param name="resultAppendList">���߼����</param>
        public abstract void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList);
        /// <summary>
        /// �¼������
        /// </summary>
        public abstract Camera eventCamera { get; }

        [Obsolete("Please use sortOrderPriority and renderOrderPriority", false)]
        public virtual int priority
        {
            get { return 0; }
        }
        /// <summary>
        /// ����˳�����ȼ�
        /// </summary>
        public virtual int sortOrderPriority
        {
            get { return int.MinValue; }
        }
        /// <summary>
        /// ��Ⱦ˳�����ȼ�
        /// </summary>
        public virtual int renderOrderPriority
        {
            get { return int.MinValue; }
        }

        public override string ToString()
        {
            return "Name: " + gameObject + "\n" +
                "eventCamera: " + eventCamera + "\n" +
                "sortOrderPriority: " + sortOrderPriority + "\n" +
                "renderOrderPriority: " + renderOrderPriority;
        }

        /// <summary>
        /// �����¼���������Ͷ������ӽ�Ͷ���߹�����
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            RaycasterManager.AddRaycaster(this);
        }

        /// <summary>
        /// ���ú�������Ͷ�����Ƴ�Ͷ���߹�����
        /// </summary>
        protected override void OnDisable()
        {
            RaycasterManager.RemoveRaycasters(this);
            base.OnDisable();
        }
    }
}
