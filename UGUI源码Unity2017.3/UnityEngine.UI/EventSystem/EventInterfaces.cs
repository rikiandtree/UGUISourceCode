namespace UnityEngine.EventSystems
{
    /// <summary>
    /// �¼�ϵͳ����ӿ�
    /// </summary>
    public interface IEventSystemHandler
    {
    }

    /// <summary>
    /// ָ������¼�
    /// </summary>
    public interface IPointerEnterHandler : IEventSystemHandler
    {
        void OnPointerEnter(PointerEventData eventData);
    }

    /// <summary>
    /// ָ���뿪�¼�
    /// </summary>
    public interface IPointerExitHandler : IEventSystemHandler
    {
        void OnPointerExit(PointerEventData eventData);
    }

    /// <summary>
    /// ָ������ʼ�¼�
    /// </summary>
    public interface IPointerDownHandler : IEventSystemHandler
    {
        void OnPointerDown(PointerEventData eventData);
    }

    /// <summary>
    /// ָ���������¼�
    /// </summary>
    public interface IPointerUpHandler : IEventSystemHandler
    {
        void OnPointerUp(PointerEventData eventData);
    }

    /// <summary>
    /// ָ�����¼�
    /// </summary>
    public interface IPointerClickHandler : IEventSystemHandler
    {
        void OnPointerClick(PointerEventData eventData);
    }

    /// <summary>
    /// ��ʼ��ק�¼�
    /// </summary>
    public interface IBeginDragHandler : IEventSystemHandler
    {
        void OnBeginDrag(PointerEventData eventData);
    }

    public interface IInitializePotentialDragHandler : IEventSystemHandler
    {
        void OnInitializePotentialDrag(PointerEventData eventData);
    }

    /// <summary>
    /// ��ק�¼�
    /// </summary>
    public interface IDragHandler : IEventSystemHandler
    {
        void OnDrag(PointerEventData eventData);
    }

    /// <summary>
    /// ������ק�¼�
    /// </summary>
    public interface IEndDragHandler : IEventSystemHandler
    {
        void OnEndDrag(PointerEventData eventData);
    }

    public interface IDropHandler : IEventSystemHandler
    {
        void OnDrop(PointerEventData eventData);
    }

    public interface IScrollHandler : IEventSystemHandler
    {
        void OnScroll(PointerEventData eventData);
    }

    public interface IUpdateSelectedHandler : IEventSystemHandler
    {
        void OnUpdateSelected(BaseEventData eventData);
    }

    /// <summary>
    /// ѡ���¼�
    /// </summary>
    public interface ISelectHandler : IEventSystemHandler
    {
        void OnSelect(BaseEventData eventData);
    }

    /// <summary>
    /// �ı�ѡ���¼�
    /// </summary>
    public interface IDeselectHandler : IEventSystemHandler
    {
        void OnDeselect(BaseEventData eventData);
    }

    /// <summary>
    /// �ƶ��¼�
    /// </summary>
    public interface IMoveHandler : IEventSystemHandler
    {
        void OnMove(AxisEventData eventData);
    }

    public interface ISubmitHandler : IEventSystemHandler
    {
        void OnSubmit(BaseEventData eventData);
    }

    /// <summary>
    /// ȡ���¼�
    /// </summary>
    public interface ICancelHandler : IEventSystemHandler
    {
        void OnCancel(BaseEventData eventData);
    }
}
