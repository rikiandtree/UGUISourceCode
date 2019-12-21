using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    //���߹�����
    internal static class RaycasterManager
    {
        //����Ͷ�����б�
        private static readonly List<BaseRaycaster> s_Raycasters = new List<BaseRaycaster>();
        
        /// <summary>
        /// ���Ͷ����
        /// </summary>
        /// <param name="baseRaycaster"></param>
        public static void AddRaycaster(BaseRaycaster baseRaycaster)
        {
            if (s_Raycasters.Contains(baseRaycaster))
                return;

            s_Raycasters.Add(baseRaycaster);
        }
        
        /// <summary>
        /// ���Ͷ�����б�
        /// </summary>
        /// <returns>Ͷ�����б�</returns>
        public static List<BaseRaycaster> GetRaycasters()
        {
            return s_Raycasters;
        }
        
        /// <summary>
        /// �Ƴ�Ͷ����
        /// </summary>
        /// <param name="baseRaycaster">Ҫ�Ƴ���Ͷ����</param>
        public static void RemoveRaycasters(BaseRaycaster baseRaycaster)
        {
            if (!s_Raycasters.Contains(baseRaycaster))
                return;
            s_Raycasters.Remove(baseRaycaster);
        }
    }
}
