using System;
using System.Collections.Generic;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
    //������������
    public enum CanvasUpdate
    {
        Prelayout = 0,
        Layout = 1,
        PostLayout = 2,
        PreRender = 3,
        LatePreRender = 4,
        MaxUpdateValue = 5
    }

    //����Ԫ�ؽӿ�
    public interface ICanvasElement
    {
        //ִ�к���
        void Rebuild(CanvasUpdate executing);
        //�����Ϣ
        Transform transform { get; }
        //���ֽ�������
        void LayoutComplete();
        //���ƽ�������
        void GraphicUpdateComplete();
        // due to unity overriding null check
        // we need this as something may not be null
        // but may be destroyed
        //�Ƿ�����
        bool IsDestroyed();
    }
    //��������ע��
    public class CanvasUpdateRegistry
    {
        //����
        private static CanvasUpdateRegistry s_Instance;
        //�Ƿ�ִ��ִ�в��ָ���
        private bool m_PerformingLayoutUpdate;
        //�Ƿ�ִ��ͼ�����
        private bool m_PerformingGraphicUpdate;
        //����Ԫ�ض���
        private readonly IndexedSet<ICanvasElement> m_LayoutRebuildQueue = new IndexedSet<ICanvasElement>();
        //ͼ��Ԫ�ض���
        private readonly IndexedSet<ICanvasElement> m_GraphicRebuildQueue = new IndexedSet<ICanvasElement>();
        //���캯��
        protected CanvasUpdateRegistry()
        {
            //������Ⱦʱ��
            Canvas.willRenderCanvases += PerformUpdate;
        }
        //����
        public static CanvasUpdateRegistry instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new CanvasUpdateRegistry();
                return s_Instance;
            }
        }
        //�ж����岻Ϊnull
        private bool ObjectValidForUpdate(ICanvasElement element)
        {
            var valid = element != null;

            var isUnityObject = element is Object;
            if (isUnityObject)
                valid = (element as Object) != null; //Here we make use of the overloaded UnityEngine.Object == null, that checks if the native object is alive.

            return valid;
        }

        //��������и�Ϊ�ջ����Ѿ����ٵ�Ԫ��
        private void CleanInvalidItems()
        {
            // So MB's override the == operator for null equality, which checks
            // if they are destroyed. This is fine if you are looking at a concrete
            // mb, but in this case we are looking at a list of ICanvasElement
            // this won't forward the == operator to the MB, but just check if the
            // interface is null. IsDestroyed will return if the backend is destroyed.

            for (int i = m_LayoutRebuildQueue.Count - 1; i >= 0; --i)
            {
                var item = m_LayoutRebuildQueue[i];
                //Ԫ��Ϊ�գ����
                if (item == null)
                {
                    m_LayoutRebuildQueue.RemoveAt(i);
                    continue;
                }

                //Ԫ���Ѿ����٣����
                if (item.IsDestroyed())
                {
                    m_LayoutRebuildQueue.RemoveAt(i);
                    //����Ԫ����ɺ���
                    item.LayoutComplete();
                }
            }

            for (int i = m_GraphicRebuildQueue.Count - 1; i >= 0; --i)
            {
                var item = m_GraphicRebuildQueue[i];
                if (item == null)
                {
                    m_GraphicRebuildQueue.RemoveAt(i);
                    continue;
                }

                if (item.IsDestroyed())
                {
                    m_GraphicRebuildQueue.RemoveAt(i);
                    item.GraphicUpdateComplete();
                }
            }
        }
        //�������
        private static readonly Comparison<ICanvasElement> s_SortLayoutFunction = SortLayoutList;
        //ִ�и���
        private void PerformUpdate()
        {
            UISystemProfilerApi.BeginSample(UISystemProfilerApi.SampleType.Layout);
            //�����Ϊ�յ�Ԫ��
            CleanInvalidItems();
            //����ִ�и���Ϊtrue
            m_PerformingLayoutUpdate = true;
            //����Ԫ�ظ����������
            m_LayoutRebuildQueue.Sort(s_SortLayoutFunction);

            //������Ⱦ
            for (int i = 0; i <= (int)CanvasUpdate.PostLayout; i++)
            {
                for (int j = 0; j < m_LayoutRebuildQueue.Count; j++)
                {
                    //���Ԫ��
                    var rebuild = instance.m_LayoutRebuildQueue[j];
                    try
                    {
                        //Ԫ�ز�Ϊ�գ���ִ����rebuild����
                        if (ObjectValidForUpdate(rebuild))
                            rebuild.Rebuild((CanvasUpdate)i);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e, rebuild.transform);
                    }
                }
            }

            //ִ��Ԫ����Ⱦ��������
            for (int i = 0; i < m_LayoutRebuildQueue.Count; ++i)
                m_LayoutRebuildQueue[i].LayoutComplete();
            //��ղ�����Ⱦ����
            instance.m_LayoutRebuildQueue.Clear();
            //ִ�в���Ϊfalse
            m_PerformingLayoutUpdate = false;

            // now layout is complete do culling...
            ClipperRegistry.instance.Cull();

            //ִ��ͼ�����Ϊtrue
            m_PerformingGraphicUpdate = true;
            for (var i = (int)CanvasUpdate.PreRender; i < (int)CanvasUpdate.MaxUpdateValue; i++)
            {
                for (var k = 0; k < instance.m_GraphicRebuildQueue.Count; k++)
                {
                    try
                    {
                        var element = instance.m_GraphicRebuildQueue[k];
                        if (ObjectValidForUpdate(element))
                            element.Rebuild((CanvasUpdate)i);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e, instance.m_GraphicRebuildQueue[k].transform);
                    }
                }
            }

            for (int i = 0; i < m_GraphicRebuildQueue.Count; ++i)
                m_GraphicRebuildQueue[i].GraphicUpdateComplete();

            instance.m_GraphicRebuildQueue.Clear();
            m_PerformingGraphicUpdate = false;
            UISystemProfilerApi.EndSample(UISystemProfilerApi.SampleType.Layout);
        }
        //����Ԫ�����
        private static int ParentCount(Transform child)
        {
            if (child == null)
                return 0;

            var parent = child.parent;
            //û�и��׾�Ϊ0
            int count = 0;
            //�游������
            while (parent != null)
            {
                count++;
                parent = parent.parent;
            }
            return count;
        }

        //�����б��������
        private static int SortLayoutList(ICanvasElement x, ICanvasElement y)
        {
            Transform t1 = x.transform;
            Transform t2 = y.transform;

            return ParentCount(t1) - ParentCount(t2);
        }
        //Ԫ����ӵ������б�
        public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
        {
            instance.InternalRegisterCanvasElementForLayoutRebuild(element);
        }
        //Ԫ����ӵ������б���ӳɹ�Ϊtrue�����ʧ��Ϊfalse��
        public static bool TryRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
        {
            return instance.InternalRegisterCanvasElementForLayoutRebuild(element);
        }
        //Ԫ����ӵ������б�
        private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
        {
            if (m_LayoutRebuildQueue.Contains(element))
                return false;

            /* TODO: this likely should be here but causes the error to show just resizing the game view (case 739376)
            if (m_PerformingLayoutUpdate)
            {
                Debug.LogError(string.Format("Trying to add {0} for layout rebuild while we are already inside a layout rebuild loop. This is not supported.", element));
                return false;
            }*/

            return m_LayoutRebuildQueue.AddUnique(element);
        }
        //����Ҫ��Ⱦ��Ԫ����ӵ�����
        public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
        {
            instance.InternalRegisterCanvasElementForGraphicRebuild(element);
        }
        //Ԫ����ӵ�ͼ���б���ӳɹ�Ϊtrue�����ʧ��Ϊfalse��
        public static bool TryRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
        {
            return instance.InternalRegisterCanvasElementForGraphicRebuild(element);
        }
        //����Ҫ��Ⱦ��Ԫ����ӵ���Ⱦ����
        private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
        {
            if (m_PerformingGraphicUpdate)
            {
                Debug.LogError(string.Format("Trying to add {0} for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.", element));
                return false;
            }

            return m_GraphicRebuildQueue.AddUnique(element);
        }
        //Ԫ�ز�ע�����Ⱦ����
        public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
        {
            instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
            instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
        }
        //���ֶ���ɾ��Ԫ��
        private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
        {
            //���ִ�в��ָ����򾯸�
            if (m_PerformingLayoutUpdate)
            {
                Debug.LogError(string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", element));
                return;
            }
            //ִ��Ԫ�ز��ֽ�������
            element.LayoutComplete();
            //���ֶ���ɾ��Ԫ��
            instance.m_LayoutRebuildQueue.Remove(element);
        }
        //��Ⱦ����ɾ��Ԫ��
        private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
        {
            if (m_PerformingGraphicUpdate)
            {
                Debug.LogError(string.Format("Trying to remove {0} from rebuild list while we are already inside a rebuild loop. This is not supported.", element));
                return;
            }
            element.GraphicUpdateComplete();
            instance.m_GraphicRebuildQueue.Remove(element);
        }
        //�Ƿ�ִ�в��ָ���
        public static bool IsRebuildingLayout()
        {
            return instance.m_PerformingLayoutUpdate;
        }
        //�Ƿ�ִ��ͼ�����
        public static bool IsRebuildingGraphics()
        {
            return instance.m_PerformingGraphicUpdate;
        }
    }
}
