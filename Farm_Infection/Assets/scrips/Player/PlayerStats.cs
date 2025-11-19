using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Valores Máximos")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float maxHunger = 100f;
    public float maxSanity = 100f;

    [Header("Valores Actuales")]
    public float health;
    public float stamina;
    public float hunger;
    public float sanity;

    [Header("Velocidades de Decaimiento")]
    public float hungerDecay = 1f;           // hambre baja siempre
    public float sanityDecay = 0.2f;         // cordura baja siempre

    [Header("Multiplicadores por Estado Crítico")]
    public float extraSanityLossLowFood = 1.0f;  // comida < 10%
    public float extraSanityLossLowHealth = 1.0f; // vida < 10%

    [Header("Daño por Hambre 0")]
    public float starvationDamage = 3f; // vida baja si comida es 0

    [Header("Regeneración con Comida Buena (>=50%)")]
    public float regenHealth = 2f;
    public float regenStamina = 8f;
    public float regenSanity = 0.5f;

    [Header("Umbrales")]
    public float regenFoodThreshold = 0.5f; // 50%
    public float lowFoodThreshold = 0.1f;   // 10%
    public float lowHealthThreshold = 0.1f; // 10%

    [Header("Respawn")]
    public Transform respawnPoint;

    private Player_Movimiento movement;

    void Start()
    {
        movement = GetComponent<Player_Movimiento>();

        health = maxHealth;
        stamina = maxStamina;
        hunger = maxHunger;
        sanity = maxSanity;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // --- 1) HAMBRE ++
        hunger -= hungerDecay * dt;
        hunger = Mathf.Clamp(hunger, 0, maxHunger);

        // --- 2) VIDA BAJA SI HAMBRE ES 0 ---
        if (hunger <= 0)
        {
            health -= starvationDamage * dt;
        }

        // --- 3) CORDURA SIEMPRE BAJA ---
        float sanityLoss = sanityDecay;

        if (hunger / maxHunger < lowFoodThreshold)
            sanityLoss += extraSanityLossLowFood;

        if (health / maxHealth < lowHealthThreshold)
            sanityLoss += extraSanityLossLowHealth;

        sanity -= sanityLoss * dt;

        // --- 4) REGENERACIÓN (SOLO SI COMIDA >= 50%) ---
        bool canRegen = hunger >= maxHunger * regenFoodThreshold;

        if (canRegen)
        {
            if (health < maxHealth)
                health += regenHealth * dt;

            if (!movement.isRunning && stamina < maxStamina)
                stamina += regenStamina * dt;

            if (sanity < maxSanity)
                sanity += regenSanity * dt;
        }

        // CLAMP FINAL
        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        sanity = Mathf.Clamp(sanity, 0, maxSanity);
        hunger = Mathf.Clamp(hunger, 0, maxHunger);

        // --- 5) REVISAR MUERTE ---
        if (health <= 0)
            Respawn();

        if (sanity <= 0)
            DeathBySanity();
    }

    // Consumir estamina (para correr / saltar)
    public bool UseStamina(float amount)
    {
        if (stamina < amount)
            return false;

        stamina -= amount;
        return true;
    }

    public void ConsumeFood(ItemData food)
    {
        if (!food.isFood) return;

        health += food.restoreHealth;
        stamina += food.restoreStamina;
        hunger += food.restoreHunger;
        sanity += food.restoreSanity;

        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        hunger = Mathf.Clamp(hunger, 0, maxHunger);
        sanity = Mathf.Clamp(sanity, 0, maxSanity);
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        health = maxHealth * 0.5f; // reapareces con 50% vida
        stamina = maxStamina;
    }

    void DeathBySanity()
    {
        movement.canMove = false;
        // Aquí ponemos UI de game over
        Debug.Log("CORDURA = 0 → PERDISTE EL JUEGO.");
    }
    public void ModifySanity(float amount)
    {
        sanity += amount;
        sanity = Mathf.Clamp(sanity, 0, maxSanity);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
