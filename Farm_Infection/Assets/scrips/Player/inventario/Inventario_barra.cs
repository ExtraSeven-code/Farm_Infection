using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Inventario_barra : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;

    private List<GameObject> slotObjects = new List<GameObject>();

    void Start()
    {
        UpdateHotbar();
        Inventario_manmager.OnChange += UpdateHotbar; // Escucha cuando cambie el inventario
    }

    void OnDestroy()
    {
        Inventario_manmager.OnChange -= UpdateHotbar;
    }

    void UpdateHotbar()
    {
        foreach (var slot in slotObjects)
            Destroy(slot);
        slotObjects.Clear();

        for (int i = 0; i < Inventario_manmager.Instance.hotbarSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotsParent);
            Image icon = newSlot.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI qty = newSlot.transform.Find("Cantidad").GetComponent<TextMeshProUGUI>();

            var slotData = Inventario_manmager.Instance.hotbar[i];
            if (slotData.item != null)
            {
                icon.sprite = slotData.item.icon;
                icon.enabled = true;
                qty.text = slotData.quantity > 1 ? slotData.quantity.ToString() : "";
            }
            else
            {
                icon.enabled = false;
                qty.text = "";
            }

            slotObjects.Add(newSlot);
        }
    }
}
