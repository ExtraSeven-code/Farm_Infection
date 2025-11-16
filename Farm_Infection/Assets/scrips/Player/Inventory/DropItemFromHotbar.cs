using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemFromHotbar : MonoBehaviour
{
    public Transform dropOrigin;        // de dónde sale el objeto (cámara o jugador)
    public float dropForwardForce = 3f;
    public float dropUpForce = 2f;

    private HotbarSelector hotbarSelector;

    private void Awake()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();
        if (dropOrigin == null)
            dropOrigin = transform; // por si se te olvida asignarlo
    }

    private void Update()
    {
        // Q para soltar como en Minecraft
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

        // 1️⃣ Restar 1 del stack en la hotbar
        bool removed = InventoryManager.Instance.RemoveItemFromSlot(true, index, 1);
        if (!removed) return;

        // 2️⃣ Instanciar el prefab delante del jugador
        Vector3 spawnPos = dropOrigin.position + dropOrigin.forward * 1f + Vector3.up * 0.5f;
        GameObject dropped = Instantiate(item.worldPrefab, spawnPos, Quaternion.identity);

        // 3️⃣ Darle un empujoncito
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
