using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public PlayerStats player;

    [Header("Barras (Image fill)")]
    public Image healthBar;
    public Image staminaBar;
    public Image hungerBar;
    public Image sanityBar;

    [Header("Textos opcionales")]
    public TMP_Text healthText;
    public TMP_Text staminaText;
    public TMP_Text hungerText;
    public TMP_Text sanityText;

    void Start()
    {
        // Si el script está en el mismo GameObject que PlayerStats
        if (player == null)
            player = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if (player == null) return;

        // Evitar divisiones raras
        float h = player.maxHealth > 0 ? player.health / player.maxHealth : 0f;
        float s = player.maxStamina > 0 ? player.stamina / player.maxStamina : 0f;
        float hu = player.maxHunger > 0 ? player.hunger / player.maxHunger : 0f;
        float sa = player.maxSanity > 0 ? player.sanity / player.maxSanity : 0f;

        // 🔹 BARRAS
        if (healthBar != null) healthBar.fillAmount = h;
        if (staminaBar != null) staminaBar.fillAmount = s;
        if (hungerBar != null) hungerBar.fillAmount = hu;
        if (sanityBar != null) sanityBar.fillAmount = sa;

        // 🔹 TEXTOS (opcional)
        if (healthText != null) healthText.text = Mathf.RoundToInt(player.health).ToString();
        if (staminaText != null) staminaText.text = Mathf.RoundToInt(player.stamina).ToString();
        if (hungerText != null) hungerText.text = Mathf.RoundToInt(player.hunger).ToString();
        if (sanityText != null) sanityText.text = Mathf.RoundToInt(player.sanity).ToString();
    }
}
