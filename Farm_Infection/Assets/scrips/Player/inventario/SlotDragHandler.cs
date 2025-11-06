using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;

    private inventario_slot slotUI;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        slotUI = GetComponent<inventario_slot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotUI == null) return;
        originalPosition = rectTransform.position;
        canvasGroup.blocksRaycasts = false; // Permite detectar drop en otros slots
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.position = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        SlotDragHandler droppedSlot = eventData.pointerDrag.GetComponent<SlotDragHandler>();
        if (droppedSlot != null && droppedSlot != this)
        {
            SwapItems(droppedSlot);
        }
    }

    private void SwapItems(SlotDragHandler other)
    {
        // Guardar datos del otro slot
        items_datos tempItem = other.slotUI.currentItem;
        int tempQuantity = other.slotUI.quantity;

        // Intercambiar
        if (slotUI.currentItem != null)
            other.slotUI.SetItem(slotUI.currentItem, slotUI.quantity);
        else
            other.slotUI.ClearSlot();

        if (tempItem != null)
            slotUI.SetItem(tempItem, tempQuantity);
        else
            slotUI.ClearSlot();
    }
}
