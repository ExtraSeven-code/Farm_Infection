using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public GameObject slotPrefab;   // Prefab del slot (igual que en hotbar)
    public Transform gridParent;    // Panel contenedor de los slots

    private List<GameObject> slotObjects = new List<GameObject>();

    private void Start()
    {
        CrearInventario();
        Inventario_manmager.OnChange += UpdateInventario; // Escucha cambios
    }

    private void OnDestroy()
    {
        Inventario_manmager.OnChange -= UpdateInventario;
    }

    void CrearInventario()
    {
        // Siempre crea 8 slots visibles
        for (int i = 0; i < Inventario_manmager.Instance.maxInventorySlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, gridParent);
            slotObjects.Add(newSlot);
        }

        // Llenamos los iconos y cantidades según inventario
        UpdateInventario();
    }

    void UpdateInventario()
    {
        for (int i = 0; i < slotObjects.Count; i++)
        {
            Image icon = slotObjects[i].transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI qty = slotObjects[i].transform.Find("Cantidad").GetComponent<TextMeshProUGUI>();

            // Si hay un ítem en el inventario
            if (i < Inventario_manmager.Instance.inventory.Count && Inventario_manmager.Instance.inventory[i].item != null)
            {
                var slotData = Inventario_manmager.Instance.inventory[i];
                icon.sprite = slotData.item.icon;
                icon.enabled = true;
                qty.text = slotData.quantity > 1 ? slotData.quantity.ToString() : "";
            }
            else
            {
                icon.enabled = false;
                qty.text = "";
            }
        }
    }
}
