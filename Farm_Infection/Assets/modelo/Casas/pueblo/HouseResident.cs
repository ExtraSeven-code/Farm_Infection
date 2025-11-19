using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseResident : MonoBehaviour
{
    [Header("Stats del habitante")]
    public float maxHealth = 100f;
    public float health = 100f;

    // "Comida" interna, en unidades arbitrarias (ej: cada papa suma 10)
    public float foodReserve = 0f;

    [Tooltip("Cuánta comida se gasta por segundo")]
    public float foodDrainPerSecond = 1f;

    [Tooltip("Vida que se regenera por segundo si hay comida")]
    public float healthRegenPerSecond = 2f;

    [Tooltip("Vida que se pierde por segundo si NO hay comida")]
    public float healthLossPerSecondWhenHungry = 3f;

    [Header("Para la barra de comida")]
    [Tooltip("Cantidad de comida que representa la barra llena (puede haber más comida interna)")]
    public float foodDisplayMax = 100f;

    [Header("Referencias")]
    public HouseUI houseUI;       // UI arriba de la puerta
    public PlayerStats playerStats; // para bajar tu cordura al morir

    private bool isDead = false;

    void Start()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (houseUI != null)
            houseUI.Refresh(this);
    }

    void Update()
    {
        if (isDead) return;

        float dt = Time.deltaTime;

        // 🥔 Tiene comida → la va gastando y regenera vida
        if (foodReserve > 0f)
        {
            float consume = foodDrainPerSecond * dt;
            foodReserve = Mathf.Max(0f, foodReserve - consume);

            if (health < maxHealth)
                health += healthRegenPerSecond * dt;
        }
        else
        {
            // 😵‍💫 Sin comida → se muere poco a poco
            health -= healthLossPerSecondWhenHungry * dt;
        }

        health = Mathf.Clamp(health, 0f, maxHealth);

        if (houseUI != null)
            houseUI.Refresh(this);

        // 💀 Muerte
        if (!isDead && health <= 0f)
        {
            isDead = true;
            OnDeath();
        }
    }

    void OnDeath()
    {
        // Bajamos ~40% de tu cordura total
        if (playerStats != null)
        {
            float loss = playerStats.maxSanity * 0.4f;
            playerStats.ModifySanity(-loss);
        }

        // Aquí puedes cambiar color de la casa, apagar luces, etc.
        Debug.Log($"{name} ha muerto. Cordura del jugador reducida.");
    }

    // 👇 Para alimentar usando ítems de comida (usarás esto desde la puerta)
    public void AddFoodFromItem(ItemData item, int amount)
    {
        if (item == null || !item.isFood || amount <= 0) return;

        // Reutilizamos restoreHunger del item como "valor nutritivo"
        float value = item.restoreHunger * amount;
        AddFood(value);
    }

    public void AddFood(float amount)
    {
        if (amount <= 0f) return;
        foodReserve += amount;
    }

    // 👉 Porcentaje para la barra de comida (capa en foodDisplayMax)
    public float GetFoodPercent()
    {
        if (foodDisplayMax <= 0f) return 0f;
        return Mathf.Clamp01(foodReserve / foodDisplayMax);
    }

    // 👉 Porcentaje para la barra de vida
    public float GetHealthPercent()
    {
        if (maxHealth <= 0f) return 0f;
        return Mathf.Clamp01(health / maxHealth);
    }
}
