using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("Panel del inventario grande")]
    public GameObject inventoryPanel;

    [Header("Tecla para abrir/cerrar")]
    public KeyCode toggleKey = KeyCode.E; // o Tab, I, como prefieras

    private bool isOpen = false;

    private void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        // Opcional: cursor bloqueado al inicio si es juego 1a/3a persona
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            // Mostrar cursor para poder hacer clic en el inventario
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Opcional: pausar juego mientras inventario está abierto
            // Time.timeScale = 0f;
        }
        else
        {
            // Volver al control normal del jugador
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Si pausaste el tiempo, lo reanudas
            // Time.timeScale = 1f;
        }
    }
}
