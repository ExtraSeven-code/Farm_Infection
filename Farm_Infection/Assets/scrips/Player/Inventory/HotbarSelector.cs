using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelector : MonoBehaviour
{
    public int currentIndex = 0;
    public int hotbarSize = 9;

    private void Update()
    {
        // Números 1–9
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentIndex = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) currentIndex = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) currentIndex = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) currentIndex = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9)) currentIndex = 8;

        // Rueda del ratón (opcional)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            currentIndex = (currentIndex + 1) % hotbarSize;
        else if (scroll < 0f)
            currentIndex = (currentIndex - 1 + hotbarSize) % hotbarSize;

        // Aquí podrías actualizar un borde de selección visual en la UI.
    }

    public ItemData GetSelectedItem()
    {
        var slot = InventoryManager.Instance.GetSlot(true, currentIndex);
        if (slot == null || slot.IsEmpty) return null;
        return slot.item;
    }
}
