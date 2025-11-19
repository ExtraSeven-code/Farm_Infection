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

        if (selected == currentEquippedItem)
            return;

        Equip(selected);
    }

    void Equip(ItemData newItem)
    {
        currentEquippedItem = newItem;

        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }

        if (newItem == null || newItem.handPrefab == null)
            return;

        currentInstance = Instantiate(newItem.handPrefab, handSocket);

        currentInstance.transform.localPosition = Vector3.zero;
        currentInstance.transform.localRotation = Quaternion.identity;
    }
}
