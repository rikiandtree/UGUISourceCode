using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    //射线管理类
    internal static class RaycasterManager
    {
        //射线投射者列表
        private static readonly List<BaseRaycaster> s_Raycasters = new List<BaseRaycaster>();
        
        /// <summary>
        /// 添加投射者
        /// </summary>
        /// <param name="baseRaycaster"></param>
        public static void AddRaycaster(BaseRaycaster baseRaycaster)
        {
            if (s_Raycasters.Contains(baseRaycaster))
                return;

            s_Raycasters.Add(baseRaycaster);
        }
        
        /// <summary>
        /// 获得投射者列表
        /// </summary>
        /// <returns>投射者列表</returns>
        public static List<BaseRaycaster> GetRaycasters()
        {
            return s_Raycasters;
        }
        
        /// <summary>
        /// 移除投射者
        /// </summary>
        /// <param name="baseRaycaster">要移除的投射者</param>
        public static void RemoveRaycasters(BaseRaycaster baseRaycaster)
        {
            if (!s_Raycasters.Contains(baseRaycaster))
                return;
            s_Raycasters.Remove(baseRaycaster);
        }
    }
}
