using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SlotUI : MonoBehaviour
{
    [Header("Config")]
    public bool isHotbar;   
    public int index;       

    [Header("Referencias UI")]
    public Image iconImage;
    public TextMeshProUGUI quantityText;

    public Image backgroundImage;

    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private void Update()
    {
        Refresh();
        UpdateSelectionVisual();
    }

    public void Refresh()
    {
        if (InventoryManager.Instance == null) return;

        var slot = InventoryManager.Instance.GetSlot(isHotbar, index);
        if (slot == null || slot.IsEmpty)
        {
            iconImage.enabled = false;
            quantityText.text = "";
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = slot.item.icon;

            quantityText.text = slot.quantity.ToString();
        }
    }
    private void UpdateSelectionVisual()
    {
        if (backgroundImage == null)
            return;

        if (!isHotbar)
        {
            backgroundImage.color = normalColor;
            return;
        }

        HotbarSelector selector = FindObjectOfType<HotbarSelector>();
        if (selector == null)
        {
            backgroundImage.color = normalColor;
            return;
        }

        if (selector.currentIndex == index)
            backgroundImage.color = selectedColor;
        else
            backgroundImage.color = normalColor;
    }
}
