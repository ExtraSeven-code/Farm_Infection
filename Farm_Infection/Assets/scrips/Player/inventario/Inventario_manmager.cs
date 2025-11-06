using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario_manmager : MonoBehaviour
{
    public static Inventario_manmager Instance;

    // 🔹 8 espacios para la mochila
    public int maxInventorySlots = 8;

    // 🔹 4 espacios para la barra rápida
    public int hotbarSlots = 4;

    public List<InventorySlot> inventory = new List<InventorySlot>();
    public List<InventorySlot> hotbar = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 🔹 Añadir ítem (primero intenta llenar la hotbar si hay espacio)
    public bool AddItem(items_datos item)
    {
        // Intentar acumular en hotbar
        foreach (var slot in hotbar)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                slot.quantity++;
                return true;
            }
        }

        // Intentar acumular en mochila
        foreach (var slot in inventory)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                slot.quantity++;
                return true;
            }
        }

        // Si hay espacio en hotbar y está vacía
        if (hotbar.Count < hotbarSlots)
        {
            hotbar.Add(new InventorySlot(item, 1));
            return true;
        }

        // Si hay espacio en mochila
        if (inventory.Count < maxInventorySlots)
        {
            inventory.Add(new InventorySlot(item, 1));
            return true;
        }

        Debug.Log("Inventario lleno");
        return false;
    }

    // 🔹 Quitar ítem (busca primero en hotbar)
    public void RemoveItem(items_datos item)
    {
        for (int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i].item == item)
            {
                hotbar[i].quantity--;
                if (hotbar[i].quantity <= 0) hotbar.RemoveAt(i);
                return;
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item == item)
            {
                inventory[i].quantity--;
                if (inventory[i].quantity <= 0) inventory.RemoveAt(i);
                return;
            }
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public items_datos item;
    public int quantity;

    public InventorySlot(items_datos newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
}
