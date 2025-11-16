using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingController : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;

    private HotbarSelector hotbarSelector;

    private void Awake()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void Update()
    {
        // Clic izquierdo -> intentar plantar
        if (Input.GetMouseButtonDown(0))
        {
            TryPlantSeed();
        }

        // F -> regar
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryWater();
        }

        // E -> cosechar
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryHarvest();
        }
    }

    bool RaycastPot(out PlantPot pot)
    {
        pot = null;
        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            pot = hit.collider.GetComponent<PlantPot>();
            if (pot == null)
                pot = hit.collider.GetComponentInParent<PlantPot>();
        }

        return pot != null;
    }

    void TryPlantSeed()
    {
        if (InventoryManager.Instance == null || hotbarSelector == null)
            return;

        // Obtener el slot activo de la hotbar
        int index = hotbarSelector.currentIndex;
        InventorySlot slot = InventoryManager.Instance.GetSlot(true, index);

        if (slot == null || slot.IsEmpty || slot.item == null || !slot.item.isSeed)
            return;

        // Ver si estamos mirando una maceta
        if (!RaycastPot(out PlantPot pot))
            return;

        if (!pot.CanPlant())
            return;

        // Intentar plantar
        bool planted = pot.TryPlant(slot.item);
        if (planted)
        {
            // Quitar 1 semilla del inventario
            InventoryManager.Instance.RemoveItemFromSlot(true, index, 1);
        }
    }

    void TryWater()
    {
        if (!RaycastPot(out PlantPot pot))
            return;

        pot.Water();
        Debug.Log("Maceta regada");
    }

    void TryHarvest()
    {
        if (InventoryManager.Instance == null)
            return;

        if (!RaycastPot(out PlantPot pot))
            return;

        if (pot.TryHarvest(out ItemData fruit, out int amount))
        {
            InventoryManager.Instance.AddItem(fruit, amount);
            Debug.Log("Cosechado: " + fruit.displayName + " x" + amount);
        }
        else
        {
            Debug.Log("La planta aún no está lista para cosechar.");
        }
    }
}
