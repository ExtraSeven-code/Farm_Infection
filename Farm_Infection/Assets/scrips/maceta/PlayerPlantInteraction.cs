using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlantInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public string interactTrigger = "Interact";

    [Header("Bloqueo de movimiento")]
    public float interactLockTime;   // dura lo que dura la animación

    private HotbarSelector hotbarSelector;
    private PlantPot currentPot;
    private Player_Movimiento playerMovimiento;

    public PlantPot CurrentPot => currentPot;

    [Header("UI")]
    public GameObject pressEPanel;

    private void Awake()
    {
        hotbarSelector = FindObjectOfType<HotbarSelector>();
        playerMovimiento = GetComponent<Player_Movimiento>();
    }

    public void SetCurrentPot(PlantPot pot)
    {
        currentPot = pot;
        if (pressEPanel != null)
            pressEPanel.SetActive(currentPot != null);
    }

    private void Update()
    {
        if (currentPot == null || InventoryManager.Instance == null || hotbarSelector == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    void HandleInteraction()
    {
        if (TryHarvest())
            return;

        if (TryPlant())
            return;

        if (TryWater())
            return;
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

        if (playerMovimiento != null)
        {
            playerMovimiento.isInteracting = true;
            StopCoroutine(nameof(UnlockAfterDelay));
            StartCoroutine(UnlockAfterDelay());
        }
    }

    IEnumerator UnlockAfterDelay()
    {
        yield return new WaitForSeconds(interactLockTime);
        if (playerMovimiento != null)
            playerMovimiento.isInteracting = false;
    }
}
