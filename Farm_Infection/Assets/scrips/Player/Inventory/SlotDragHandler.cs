using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDragHandler : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Referencia al SlotUI de este slot")]
    public SlotUI slotUI;

    private Canvas canvas;
    private CanvasGroup canvasGroup;

    // Visual del objeto que se arrastra
    private RectTransform dragVisual;
    private Image dragIcon;
    private TextMeshProUGUI dragQuantity;

    // Slot desde el que estamos arrastrando (compartido por todos)
    public static SlotDragHandler draggingSlot;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (slotUI == null)
            slotUI = GetComponent<SlotUI>();
    }

    // 🔹 Empezar a arrastrar
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null || slotUI == null)
            return;

        var slot = InventoryManager.Instance.GetSlot(slotUI.isHotbar, slotUI.index);

        // No hay nada en este slot → no arrastramos
        if (slot == null || slot.IsEmpty)
            return;

        draggingSlot = this;

        // Para que los raycasts pasen "a través" mientras arrastramos
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;

        // Crear objeto visual que sigue al mouse
        dragVisual = new GameObject("DragVisual").AddComponent<RectTransform>();
        dragVisual.SetParent(canvas.transform, false);
        dragVisual.sizeDelta = ((RectTransform)transform).sizeDelta;
        dragVisual.position = eventData.position;

        // Icono
        dragIcon = new GameObject("Icon").AddComponent<Image>();
        dragIcon.transform.SetParent(dragVisual, false);
        dragIcon.sprite = slot.item.icon;
        dragIcon.raycastTarget = false;

        // Cantidad
        dragQuantity = new GameObject("Quantity").AddComponent<TextMeshProUGUI>();
        dragQuantity.transform.SetParent(dragVisual, false);
        dragQuantity.alignment = TextAlignmentOptions.BottomRight;
        dragQuantity.fontSize = 24;
        dragQuantity.text = slot.quantity.ToString();
        dragQuantity.raycastTarget = false;
    }

    // 🔹 Mientras arrastramos
    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual == null) return;
        dragVisual.position = eventData.position;
    }

    // 🔹 Soltamos el mouse (da igual dónde)
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingSlot = null;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (dragVisual != null)
            Destroy(dragVisual.gameObject);
    }

    // 🔹 Se suelta algo encima de este slot
    public void OnDrop(PointerEventData eventData)
    {
        if (draggingSlot == null || draggingSlot == this)
            return;

        if (InventoryManager.Instance == null)
            return;

        // Intercambiar contenido entre el slot de origen y este slot
        InventoryManager.Instance.SwapSlots(
            draggingSlot.slotUI.isHotbar, draggingSlot.slotUI.index,
            this.slotUI.isHotbar, this.slotUI.index
        );

        // Los SlotUI se actualizan solos en Update() con Refresh()
    }
}
