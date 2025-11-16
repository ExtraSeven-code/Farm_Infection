using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Tamaño del inventario")]
    public int inventorySize = 27;   // 3 filas de 9, por ejemplo
    public int hotbarSize = 9;

    [Header("Slots")]
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public List<InventorySlot> hotbar = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Inicializar listas
        if (inventory.Count != inventorySize)
        {
            inventory = new List<InventorySlot>();
            for (int i = 0; i < inventorySize; i++)
                inventory.Add(new InventorySlot());
        }

        if (hotbar.Count != hotbarSize)
        {
            hotbar = new List<InventorySlot>();
            for (int i = 0; i < hotbarSize; i++)
                hotbar.Add(new InventorySlot());
        }
    }

    // 🔹 Añadir un ítem al inventario (primero intenta apilar, luego usar slots vacíos)
    public bool AddItem(ItemData item, int amount = 1)
    {
        int restante = amount;

        // 1) Intentar apilar en la hotbar
        restante = TryAddToList(hotbar, restante, item);
        if (restante <= 0) return true;

        // 2) Intentar apilar en inventario interno
        restante = TryAddToList(inventory, restante, item);
        if (restante <= 0) return true;

        // 3) Usar slots vacíos en hotbar
        restante = TryAddToEmptySlots(hotbar, restante, item);
        if (restante <= 0) return true;

        // 4) Usar slots vacíos en inventario interno
        restante = TryAddToEmptySlots(inventory, restante, item);
        return restante <= 0; // true = se añadió todo, false = faltó espacio
    }

    private int TryAddToList(List<InventorySlot> list, int amount, ItemData item)
    {
        foreach (var slot in list)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                int espacio = item.maxStack - slot.quantity;
                int aAgregar = Mathf.Min(espacio, amount);
                slot.quantity += aAgregar;
                amount -= aAgregar;

                if (amount <= 0) break;
            }
        }
        return amount;
    }

    private int TryAddToEmptySlots(List<InventorySlot> list, int amount, ItemData item)
    {
        foreach (var slot in list)
        {
            if (slot.IsEmpty)
            {
                int aAgregar = Mathf.Min(item.maxStack, amount);
                slot.item = item;
                slot.quantity = aAgregar;
                amount -= aAgregar;

                if (amount <= 0) break;
            }
        }
        return amount;
    }

    // 🔹 Obtener un slot concreto (para la UI)
    public InventorySlot GetSlot(bool fromHotbar, int index)
    {
        if (fromHotbar)
        {
            if (index < 0 || index >= hotbar.Count) return null;
            return hotbar[index];
        }
        else
        {
            if (index < 0 || index >= inventory.Count) return null;
            return inventory[index];
        }
    }

    // 🔹 Intercambiar dos slots (inventario ↔ hotbar o dentro del mismo)
    public void SwapSlots(bool firstHotbar, int firstIndex, bool secondHotbar, int secondIndex)
    {
        InventorySlot a = GetSlot(firstHotbar, firstIndex);
        InventorySlot b = GetSlot(secondHotbar, secondIndex);
        if (a == null || b == null) return;

        ItemData tempItem = b.item;
        int tempQty = b.quantity;

        b.item = a.item;
        b.quantity = a.quantity;

        a.item = tempItem;
        a.quantity = tempQty;
    }
}
