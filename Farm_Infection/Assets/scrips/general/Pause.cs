using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("UI panel Escape")]
    public GameObject PanelEscape;

    private Player_Movimiento playerMovimiento;
    private bool isPaused = false;

    private void Start()
    {
        playerMovimiento = FindObjectOfType<Player_Movimiento>();

        PanelEscape.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // ACTIVA PAUSA
    void PauseGame()
    {
        isPaused = true;

        PanelEscape.SetActive(true);
        playerMovimiento.canMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;  
    }

    public void ResumeGame()
    {
        isPaused = false;

        PanelEscape.SetActive(false);
        playerMovimiento.canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f; 
    }
}
