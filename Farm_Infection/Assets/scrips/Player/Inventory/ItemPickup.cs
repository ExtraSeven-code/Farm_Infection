using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item que va a dar")]
    public ItemData item;  
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("No hay InventoryManager en la escena.");
            return;
        }

        bool added = InventoryManager.Instance.AddItem(item, amount);

        if (added)
        {
            
            Destroy(gameObject); 
        }
        else
        {
            Debug.Log("Inventario lleno, no se pudo recoger el item.");
        }
    }
}
