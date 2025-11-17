using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemDrop : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (item == null) return;
        if (InventoryManager.Instance == null) return;

        // Intentar añadir al inventario
        bool added = InventoryManager.Instance.AddItem(item, amount);

        if (added)
        {
            Destroy(gameObject);
        }
        else
        {
            // Si el inventario está lleno, podrías dejarlo ahí
            Debug.Log("Inventario lleno, no se pudo recoger " + item.displayName);
        }
    }
}
