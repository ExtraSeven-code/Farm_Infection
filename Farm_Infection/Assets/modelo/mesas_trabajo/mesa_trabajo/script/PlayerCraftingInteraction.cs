using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftingInteraction : MonoBehaviour
{
    public GameObject pressEPanelForCrafting;

    private CraftingTable currentTable;
    public CraftingTable CurrentTable => currentTable;

    private Player_Movimiento playerMovimiento; // para bloquear movimiento opcional

    private void Awake()
    {
        playerMovimiento = GetComponent<Player_Movimiento>();
    }

    public void SetCurrentTable(CraftingTable table)
    {
        currentTable = table;

        if (pressEPanelForCrafting != null)
            pressEPanelForCrafting.SetActive(currentTable != null);
    }

    private void Update()
    {
        if (currentTable == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isOpen = currentTable.craftingPanel != null &&
                          currentTable.craftingPanel.activeSelf;

            if (isOpen)
                CloseCrafting();
            else
                OpenCrafting();
        }
    }

    void OpenCrafting()
    {
        currentTable.OpenUI();

        // 🔓 Mostrar el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Opcional: bloquear movimiento del player
        if (playerMovimiento != null)
            playerMovimiento.isInteracting = true;
    }

    void CloseCrafting()
    {
        currentTable.CloseUI();

        // 🔒 Volver al modo juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovimiento != null)
            playerMovimiento.isInteracting = false;
    }
}
