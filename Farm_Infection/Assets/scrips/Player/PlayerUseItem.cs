using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    public HotbarSelector hotbar;
    public PlayerStats stats;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ItemData item = hotbar.GetSelectedItem();
            if (item == null) return;

            if (item.isFood)
            {
                stats.ConsumeFood(item);

                InventoryManager.Instance.RemoveItemFromSlot(true, hotbar.currentIndex, 1);
            }

            
        }
    }
}
