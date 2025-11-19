using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoorInteraction : MonoBehaviour
{
    public HouseResident house;
    public GameObject feedPanel;        
    public GameObject pressEIndicator;

    private bool playerInside = false;
    private bool panelOpen = false;

    private HouseFoodSlot foodSlot;     

    void Start()
    {
        if (feedPanel != null)
        {
            feedPanel.SetActive(false);
            foodSlot = feedPanel.GetComponentInChildren<HouseFoodSlot>(true);
        }

        if (pressEIndicator != null)
            pressEIndicator.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!panelOpen)
                OpenPanel();
            else
                ClosePanel();
        }
    }

    void OpenPanel()
    {
        panelOpen = true;
        if (feedPanel != null)
            feedPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var player = FindObjectOfType<Player_Movimiento>();
        if (player != null) player.canMove = false;
    }

    void ClosePanel()
    {
        panelOpen = false;
        if (feedPanel != null)
            feedPanel.SetActive(false);

        if (foodSlot != null)
            foodSlot.ClearVisual();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var player = FindObjectOfType<Player_Movimiento>();
        if (player != null) player.canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        if (pressEIndicator != null)
            pressEIndicator.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        ClosePanel();

        if (pressEIndicator != null)
            pressEIndicator.SetActive(false);
    }


}
