using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recolectar_item : MonoBehaviour
{
    public items_datos itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = Inventario_manmager.Instance.AddItem(itemData);
            if (added)
            {
                Debug.Log("Recogido: " + itemData.itemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventario lleno, no se pudo recoger " + itemData.itemName);
            }
        }
    }
}
