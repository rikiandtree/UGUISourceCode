namespace UnityEngine.EventSystems
{
    /// <summary>
    /// 事件系统处理接口
    /// </summary>
    public interface IEventSystemHandler
    {
    }

    /// <summary>
    /// 指针进入事件
    /// </summary>
    public interface IPointerEnterHandler : IEventSystemHandler
    {
        void OnPointerEnter(PointerEventData eventData);
    }

    /// <summary>
    /// 指针离开事件
    /// </summary>
    public interface IPointerExitHandler : IEventSystemHandler
    {
        void OnPointerExit(PointerEventData eventData);
    }

    /// <summary>
    /// 指针点击开始事件
    /// </summary>
    public interface IPointerDownHandler : IEventSystemHandler
    {
        void OnPointerDown(PointerEventData eventData);
    }

    /// <summary>
    /// 指针点击结束事件
    /// </summary>
    public interface IPointerUpHandler : IEventSystemHandler
    {
        void OnPointerUp(PointerEventData eventData);
    }

    /// <summary>
    /// 指针点击事件
    /// </summary>
    public interface IPointerClickHandler : IEventSystemHandler
    {
        void OnPointerClick(PointerEventData eventData);
    }

    /// <summary>
    /// 开始拖拽事件
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
    /// 拖拽事件
    /// </summary>
    public interface IDragHandler : IEventSystemHandler
    {
        void OnDrag(PointerEventData eventData);
    }

    /// <summary>
    /// 结束拖拽事件
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
    /// 选择事件
    /// </summary>
    public interface ISelectHandler : IEventSystemHandler
    {
        void OnSelect(BaseEventData eventData);
    }

    /// <summary>
    /// 改变选择事件
    /// </summary>
    public interface IDeselectHandler : IEventSystemHandler
    {
        void OnDeselect(BaseEventData eventData);
    }

    /// <summary>
    /// 移动事件
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
    /// 取消事件
    /// </summary>
    public interface ICancelHandler : IEventSystemHandler
    {
        void OnCancel(BaseEventData eventData);
    }
}
