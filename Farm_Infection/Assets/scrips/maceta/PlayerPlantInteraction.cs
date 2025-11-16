using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlantInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;                 // Animator del jugador
    public string interactTrigger = "Interact";  // Trigger en el Animator

    private HotbarSelector hotbarSelector;
    private PlantPot currentPot;

    public PlantPot CurrentPot => currentPot;

    private Player_Movimiento playerMovimiento;

    private void Awake()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();
        playerMovimiento = GetComponent<Player_Movimiento>();
    }

    public void SetCurrentPot(PlantPot pot)
    {
        currentPot = pot;
        // Aquí podrías mostrar/ocultar un cartel "Presiona E para interactuar"
    }

    private void Update()
    {
        if (currentPot == null || InventoryManager.Instance == null || hotbarSelector == null)
            return;

        // ✅ TODAS las interacciones con E
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    void HandleInteraction()
    {
        // 1️⃣ Primero: intentar COSECHAR (prioridad máxima)
        if (TryHarvest())
            return;

        // 2️⃣ Después: intentar PLANTAR (si la maceta está vacía y tienes semilla)
        if (TryPlant())
            return;

        // 3️⃣ Finalmente: intentar REGAR (si tienes herramienta de riego)
        if (TryWater())
            return;

        // Si nada aplica, no pasa nada (puedes poner un sonido de "no se puede")
    }

    bool TryPlant()
    {
        int index = hotbarSelector.currentIndex;
        InventorySlot slot = InventoryManager.Instance.GetSlot(true, index);

        if (slot == null || slot.IsEmpty || slot.item == null || !slot.item.isSeed)
            return false;

        if (!currentPot.CanPlant())
            return false;

        bool planted = currentPot.TryPlant(slot.item);
        if (planted)
        {
            InventoryManager.Instance.RemoveItemFromSlot(true, index, 1);
            PlayInteractAnimation();
            playerMovimiento.isInteracting = true;
            return true;
        }

        return false;
    }

    bool TryWater()
    {
        int index = hotbarSelector.currentIndex;
        InventorySlot slot = InventoryManager.Instance.GetSlot(true, index);

        if (slot == null || slot.IsEmpty || slot.item == null || !slot.item.isWateringTool)
            return false;

        currentPot.Water();
        PlayInteractAnimation();
        playerMovimiento.isInteracting = true;
        return true;
    }

    bool TryHarvest()
    {
        if (currentPot.TryHarvest(out ItemData fruit, out int amount))
        {
            if (fruit != null && amount > 0)
            {
                InventoryManager.Instance.AddItem(fruit, amount);
            }
            PlayInteractAnimation();
            playerMovimiento.isInteracting = true;
            return true;
        }

        return false;
    }

    void PlayInteractAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(interactTrigger))
        {
            animator.SetTrigger(interactTrigger);
        }
    }
    public void InteractionFinished()
    {
        if (playerMovimiento != null)
            playerMovimiento.isInteracting = false;
    }
}
