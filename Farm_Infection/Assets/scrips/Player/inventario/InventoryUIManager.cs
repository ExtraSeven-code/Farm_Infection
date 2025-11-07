using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform InventoryPanel;
    public Transform HotbarPanel;

    private void Start()
    {
        // Slots del inventario
        for (int i = 0; i < Inventario_manmager.Instance.inventory.Count; i++)
        {
            inventario_slot slotUI = Instantiate(slotPrefab, InventoryPanel).GetComponent<inventario_slot>();
            slotUI.SetItem(Inventario_manmager.Instance.inventory[i].item,
                           Inventario_manmager.Instance.inventory[i].quantity);

            SlotDragHandler dragHandler = slotUI.gameObject.AddComponent<SlotDragHandler>();
            dragHandler.slotIndex = i;
            dragHandler.isHotbarSlot = false;
        }

        // Slots de la hotbar
        for (int i = 0; i < Inventario_manmager.Instance.hotbar.Count; i++)
        {
            inventario_slot slotUI = Instantiate(slotPrefab, HotbarPanel).GetComponent<inventario_slot>();
            slotUI.SetItem(Inventario_manmager.Instance.hotbar[i].item,
                           Inventario_manmager.Instance.hotbar[i].quantity);

            SlotDragHandler dragHandler = slotUI.gameObject.AddComponent<SlotDragHandler>();
            dragHandler.slotIndex = i;
            dragHandler.isHotbarSlot = true;
        }
    }
}