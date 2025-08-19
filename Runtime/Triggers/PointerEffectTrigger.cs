using UnityEngine.EventSystems;

namespace Slothsoft.Effects.Triggers {
    /// <summary>
    /// Port of <see cref="EventTrigger"/> for use with <see cref="EffectEvent"/>.
    /// </summary>
    sealed class PointerEffectTrigger :
        EffectTriggerBase<EventTriggerType>,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler,
        IInitializePotentialDragHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IScrollHandler,
        IUpdateSelectedHandler,
        ISelectHandler,
        IDeselectHandler,
        IMoveHandler,
        ISubmitHandler,
        ICancelHandler {

        /// <summary>
        /// Called by the EventSystem when the pointer enters the object associated with this EventTrigger.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData) {
            Execute(EventTriggerType.PointerEnter, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when the pointer exits the object associated with this EventTrigger.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData) {
            Execute(EventTriggerType.PointerExit, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem every time the pointer is moved during dragging.
        /// </summary>
        public void OnDrag(PointerEventData eventData) {
            Execute(EventTriggerType.Drag, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when an object accepts a drop.
        /// </summary>
        public void OnDrop(PointerEventData eventData) {
            Execute(EventTriggerType.Drop, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a PointerDown event occurs.
        /// </summary>
        public void OnPointerDown(PointerEventData eventData) {
            Execute(EventTriggerType.PointerDown, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a PointerUp event occurs.
        /// </summary>
        public void OnPointerUp(PointerEventData eventData) {
            Execute(EventTriggerType.PointerUp, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a Click event occurs.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData) {
            Execute(EventTriggerType.PointerClick, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a Select event occurs.
        /// </summary>
        public void OnSelect(BaseEventData eventData) {
            Execute(EventTriggerType.Select, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a new object is being selected.
        /// </summary>
        public void OnDeselect(BaseEventData eventData) {
            Execute(EventTriggerType.Deselect, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a new Scroll event occurs.
        /// </summary>
        public void OnScroll(PointerEventData eventData) {
            Execute(EventTriggerType.Scroll, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a Move event occurs.
        /// </summary>
        public void OnMove(AxisEventData eventData) {
            Execute(EventTriggerType.Move, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when the object associated with this EventTrigger is updated.
        /// </summary>
        public void OnUpdateSelected(BaseEventData eventData) {
            Execute(EventTriggerType.UpdateSelected, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a drag has been found, but before it is valid to begin the drag.
        /// </summary>
        public void OnInitializePotentialDrag(PointerEventData eventData) {
            Execute(EventTriggerType.InitializePotentialDrag, eventData.selectedObject);
        }

        /// <summary>
        /// Called before a drag is started.
        /// </summary>
        public void OnBeginDrag(PointerEventData eventData) {
            Execute(EventTriggerType.BeginDrag, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem once dragging ends.
        /// </summary>
        public void OnEndDrag(PointerEventData eventData) {
            Execute(EventTriggerType.EndDrag, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a Submit event occurs.
        /// </summary>
        public void OnSubmit(BaseEventData eventData) {
            Execute(EventTriggerType.Submit, eventData.selectedObject);
        }

        /// <summary>
        /// Called by the EventSystem when a Cancel event occurs.
        /// </summary>
        public void OnCancel(BaseEventData eventData) {
            Execute(EventTriggerType.Cancel, eventData.selectedObject);
        }
    }
}