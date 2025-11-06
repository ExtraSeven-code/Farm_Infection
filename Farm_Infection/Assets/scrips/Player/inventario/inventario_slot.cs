using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class inventario_slot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    public items_datos currentItem { get; private set; }

    public int quantity { get; private set; }


    public void SetItem(items_datos item, int amount)
    {
        currentItem = item;
        quantity = amount;

        icon.sprite = item.icon;
        icon.enabled = true;

        quantityText.text = amount > 1 ? amount.ToString() : "";
    }

    public void ClearSlot()
    {
        currentItem = null;
        quantity = 0;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }
}
