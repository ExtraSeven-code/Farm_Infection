using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hodbart_selector : MonoBehaviour
{
    public int selectedSlot = 0;
    public Transform slotsParent;
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
    }

    void SelectSlot(int index)
    {
        selectedSlot = index;

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            Image border = slotsParent.GetChild(i).GetComponent<Image>();
            border.color = (i == selectedSlot) ? selectedColor : normalColor;
        }
    }

    public items_datos GetSelectedItem()
    {
        if (selectedSlot < Inventario_manmager.Instance.hotbar.Count)
            return Inventario_manmager.Instance.hotbar[selectedSlot].item;
        return null;
    }
}
