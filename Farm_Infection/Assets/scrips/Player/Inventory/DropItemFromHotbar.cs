using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemFromHotbar : MonoBehaviour
{
    public Transform dropOrigin;        
    public float dropForwardForce = 3f;
    public float dropUpForce = 2f;

    private HotbarSelector hotbarSelector;

    private void Awake()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();
        if (dropOrigin == null)
            dropOrigin = transform; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropSelectedItem();
        }
    }

    private void DropSelectedItem()
    {
        if (InventoryManager.Instance == null || hotbarSelector == null)
            return;

        int index = hotbarSelector.currentIndex;

        InventorySlot slot = InventoryManager.Instance.GetSlot(true, index);
        if (slot == null || slot.IsEmpty)
            return;

        ItemData item = slot.item;
        if (item.worldPrefab == null)
        {
            Debug.LogWarning("El item " + item.displayName + " no tiene worldPrefab asignado.");
            return;
        }

        bool removed = InventoryManager.Instance.RemoveItemFromSlot(true, index, 1);
        if (!removed) return;

        Vector3 spawnPos = dropOrigin.position + dropOrigin.forward * 1f + Vector3.up * 0.5f;
        GameObject dropped = Instantiate(item.worldPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(
                dropOrigin.forward * dropForwardForce +
                Vector3.up * dropUpForce,
                ForceMode.Impulse
            );
        }
    }
}
