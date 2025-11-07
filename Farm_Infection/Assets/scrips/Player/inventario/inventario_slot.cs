using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        UpdateSlotUI();
    }

    public void ClearSlot()
    {
        currentItem = null;
        quantity = 0;
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        if (currentItem != null)
        {
            icon.sprite = currentItem.icon;
            icon.enabled = true;
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            quantityText.text = "";
        }
    }
}