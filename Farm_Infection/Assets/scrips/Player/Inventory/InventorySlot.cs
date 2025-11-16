using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public bool IsEmpty => item == null || quantity <= 0;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

    public bool CanStack(ItemData newItem)
    {
        return item == newItem && quantity < item.maxStack;
    }
}
