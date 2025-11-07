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
        while (hotbar.Count < hotbarSlots)
        {
            hotbar.Add(new InventorySlot(null, 0));
        }
        while (inventory.Count < maxInventorySlots)
        {
            inventory.Add(new InventorySlot(null, 0));
        }
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
                OnChange?.Invoke();
                return true;
            }
        }

        // Intentar llenar un slot vacío en hotbar
        foreach (var slot in hotbar)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.quantity = 1;
                OnChange?.Invoke();
                return true;
            }
        }

        // 3️⃣ Intentar acumular en mochila
        foreach (var slot in inventory)
        {
            if (slot.item == item && slot.quantity < item.maxStack)
            {
                slot.quantity++;
                OnChange?.Invoke();
                return true;
            }
        }

        // 4️⃣ Intentar llenar un slot vacío en mochila
        foreach (var slot in inventory)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.quantity = 1;
                OnChange?.Invoke();
                return true;
            }
        }

        // Si hay espacio en mochila


        Debug.Log("Inventario lleno");
        return false;
    }
    public static void NotifyChange()
    {
        OnChange?.Invoke();
    }

    // 🔹 Quitar ítem (busca primero en hotbar)
    public void RemoveItem(items_datos item)
    {
        foreach (var slot in hotbar)
        {
            if (slot.item == item)
            {
                slot.quantity--;
                if (slot.quantity <= 0)
                {
                    slot.item = null;
                    slot.quantity = 0;
                }
                OnChange?.Invoke();
                return;
            }
        }

        // Después mochila
        foreach (var slot in inventory)
        {
            if (slot.item == item)
            {
                slot.quantity--;
                if (slot.quantity <= 0)
                {
                    slot.item = null;
                    slot.quantity = 0;
                }
                OnChange?.Invoke();
                return;
            }
        }
    }
    public static event System.Action OnChange;
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
