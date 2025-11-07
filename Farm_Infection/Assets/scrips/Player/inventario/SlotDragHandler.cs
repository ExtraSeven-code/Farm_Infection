using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SlotDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Configuración del Slot")]
    public int slotIndex;        // índice del slot en la lista
    public bool isHotbarSlot;    // true si es hotbar, false si es inventario

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject dragVisual;
    private inventario_slot slotComponent;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        slotComponent = GetComponent<inventario_slot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotComponent == null || slotComponent.currentItem == null)
            return;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;

        // Crear dragVisual
        dragVisual = new GameObject("DragVisual");
        dragVisual.transform.SetParent(canvas.transform, false);
        dragVisual.transform.SetAsLastSibling();

        Image img = dragVisual.AddComponent<Image>();
        img.sprite = slotComponent.currentItem.icon;
        img.raycastTarget = false;
        img.preserveAspect = true;

        RectTransform rt = dragVisual.GetComponent<RectTransform>();
        rt.sizeDelta = slotComponent.icon.rectTransform.sizeDelta;
        rt.position = slotComponent.icon.transform.position;

        // Cantidad
        if (slotComponent.quantity > 1)
        {
            GameObject textObj = new GameObject("QuantityText");
            textObj.transform.SetParent(dragVisual.transform, false);
            TextMeshProUGUI qtyText = textObj.AddComponent<TextMeshProUGUI>();
            qtyText.text = slotComponent.quantity.ToString();
            qtyText.alignment = TextAlignmentOptions.Center;
            qtyText.fontSize = 24;
            qtyText.color = Color.white;
            qtyText.enableAutoSizing = true;

            RectTransform tr = textObj.GetComponent<RectTransform>();
            tr.sizeDelta = rt.sizeDelta;
            tr.localPosition = Vector3.zero;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
            dragVisual.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragVisual != null)
            Destroy(dragVisual);

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Asegurar que el slot se vea correctamente
        slotComponent.UpdateSlotUI();
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
        // Listas reales según si es hotbar o inventario
        var listThis = isHotbarSlot ? Inventario_manmager.Instance.hotbar : Inventario_manmager.Instance.inventory;
        var listOther = other.isHotbarSlot ? Inventario_manmager.Instance.hotbar : Inventario_manmager.Instance.inventory;

        // Asegurar que existan los slots
        while (slotIndex >= listThis.Count)
            listThis.Add(new InventorySlot(null, 0));
        while (other.slotIndex >= listOther.Count)
            listOther.Add(new InventorySlot(null, 0));

        var mySlot = listThis[slotIndex];
        var otherSlot = listOther[other.slotIndex];

        // Mover o intercambiar
        if (otherSlot.item == null)
        {
            // Otro slot vacío -> mover item
            otherSlot.item = mySlot.item;
            otherSlot.quantity = mySlot.quantity;

            mySlot.item = null;
            mySlot.quantity = 0;
        }
        else if (mySlot.item == null)
        {
            // Mi slot vacío -> recibir item del otro
            mySlot.item = otherSlot.item;
            mySlot.quantity = otherSlot.quantity;

            otherSlot.item = null;
            otherSlot.quantity = 0;
        }
        else
        {
            // Ambos slots llenos -> intercambiar
            var tempItem = otherSlot.item;
            var tempQty = otherSlot.quantity;

            otherSlot.item = mySlot.item;
            otherSlot.quantity = mySlot.quantity;

            mySlot.item = tempItem;
            mySlot.quantity = tempQty;
        }

        // Actualizar la UI de ambos slots
        slotComponent.SetItem(mySlot.item, mySlot.quantity);
        other.slotComponent.SetItem(otherSlot.item, otherSlot.quantity);

        // Notificar cambios globales
        Inventario_manmager.NotifyChange();
    }
}