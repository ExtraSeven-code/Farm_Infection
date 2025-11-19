using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HouseFoodSlot : MonoBehaviour, IDropHandler
{
    [Header("Casa que va a consumir comida")]
    public HouseResident house;

    [Header("UI del slot de la casa")]
    public Image itemIcon;
    public TMP_Text amountText;

    // Este no es inventario real, solo visual temporal
    private InventorySlot tempSlot = new InventorySlot();

    public void OnDrop(PointerEventData eventData)
    {
        // Slot desde el que ARRASTRAMOS
        SlotDragHandler fromSlot = SlotDragHandler.draggingSlot;

        if (fromSlot == null || fromSlot.slotUI == null)
            return;

        int index = fromSlot.slotUI.index;
        bool isHotbar = fromSlot.slotUI.isHotbar;

        // Obtener el slot REAL desde InventoryManager
        InventorySlot originalSlot = InventoryManager.Instance.GetSlot(isHotbar, index);
        if (originalSlot == null || originalSlot.IsEmpty)
            return;

        // Solo aceptar comida
        if (!originalSlot.item.isFood)
            return;

        // 👇 Guardamos referencia ANTES de tocar el inventario
        ItemData droppedItem = originalSlot.item;
        int amount = originalSlot.quantity;
        if (amount <= 0)
            return;

        // 1️⃣ Dar comida a la casa
        if (house != null)
            house.AddFoodFromItem(droppedItem, amount);

        // 2️⃣ Eliminar del inventario del jugador
        InventoryManager.Instance.RemoveItemFromSlot(isHotbar, index, amount);

        // 3️⃣ Visual temporal (usando la referencia guardada)
        tempSlot.item = droppedItem;
        tempSlot.quantity = amount;

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (tempSlot.IsEmpty)
        {
            if (itemIcon != null)
                itemIcon.enabled = false;
            if (amountText != null)
                amountText.text = "";
        }
        else
        {
            if (itemIcon != null)
            {
                itemIcon.enabled = true;
                itemIcon.sprite = tempSlot.item.icon;
                itemIcon.color = Color.white; // por si acaso
            }

            if (amountText != null)
                amountText.text = tempSlot.quantity.ToString();
        }
    }

    public void ClearVisual()
    {
        tempSlot.Clear();
        RefreshUI();
    }
}
