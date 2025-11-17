using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    public HotbarSelector hotbar;
    public PlayerStats stats;

    void Update()
    {
        // Click derecho = usar/consumir ítem
        if (Input.GetMouseButtonDown(1))
        {
            ItemData item = hotbar.GetSelectedItem();
            if (item == null) return;

            // Si es comida → consumir
            if (item.isFood)
            {
                stats.ConsumeFood(item);

                // Quitar 1 del stack en la hotbar
                InventoryManager.Instance.RemoveItemFromSlot(true, hotbar.currentIndex, 1);
            }

            // Aquí luego puedes distinguir:
            // - si es bloque colocable → BlockPlacer
            // - si es poción, etc.
        }
    }
}
