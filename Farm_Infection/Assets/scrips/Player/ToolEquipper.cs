using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolEquipper : MonoBehaviour
{
    [Header("Dónde se pone el arma/herramienta")]
    public Transform handSocket;   // el HandSocket en la mano

    private HotbarSelector hotbarSelector;
    private ItemData currentEquippedItem;
    private GameObject currentInstance;

    private void Start()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();

        if (handSocket == null)
        {
            Debug.LogWarning("ToolEquipper: No se asignó handSocket, así que no se podrá ver el arma en la mano.");
        }
    }

    private void Update()
    {
        if (hotbarSelector == null || handSocket == null)
            return;

        ItemData selected = hotbarSelector.GetSelectedItem();

        // Si es el mismo item, no hacemos nada
        if (selected == currentEquippedItem)
            return;

        // Si cambió el ítem → reequipar
        Equip(selected);
    }

    void Equip(ItemData newItem)
    {
        currentEquippedItem = newItem;

        // Destruir el modelo anterior
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }

        // Si no hay item, o no tiene modelo en mano, no instanciamos nada
        if (newItem == null || newItem.handPrefab == null)
            return;

        // Instanciar el modelo en la mano
        currentInstance = Instantiate(newItem.handPrefab, handSocket);

        // Resetear posición/rotación local (ajusta desde el prefab o desde el socket)
        currentInstance.transform.localPosition = Vector3.zero;
        currentInstance.transform.localRotation = Quaternion.identity;
    }
}
