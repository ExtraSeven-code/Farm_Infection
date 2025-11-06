using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario_barra : MonoBehaviour
{
    public int selectedIndex = 0;
    private Inventario_manmager inventory;

    void Start()
    {
        inventory = Inventario_manmager.Instance;
    }

    void Update()
    {
        for (int i = 0; i < inventory.hotbarSlots; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                selectedIndex = i;
                Debug.Log("Slot seleccionado: " + selectedIndex);
            }
        }
    }

    public items_datos GetSelectedItem()
    {
        if (selectedIndex < inventory.hotbar.Count)
            return inventory.hotbar[selectedIndex].item;
        return null;
    }
}
