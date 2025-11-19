using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float hungerDecay = 1f;           
    public float sanityDecay = 0.2f;         

    [Header("Multiplicadores por Estado Crítico")]
    public float extraSanityLossLowFood = 1.0f;  
    public float extraSanityLossLowHealth = 1.0f; 

    [Header("Daño por Hambre 0")]
    public float starvationDamage = 3f; 
    [Header("Regeneración con Comida Buena (>=50%)")]
    public float regenHealth = 2f;
    public float regenStamina = 8f;
    public float regenSanity = 0.5f;

    [Header("Umbrales")]
    public float regenFoodThreshold = 0.5f; 
    public float lowFoodThreshold = 0.1f;   
    public float lowHealthThreshold = 0.1f; 

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

        hunger -= hungerDecay * dt;
        hunger = Mathf.Clamp(hunger, 0, maxHunger);

        if (hunger <= 0)
        {
            health -= starvationDamage * dt;
        }

        float sanityLoss = sanityDecay;

        if (hunger / maxHunger < lowFoodThreshold)
            sanityLoss += extraSanityLossLowFood;

        if (health / maxHealth < lowHealthThreshold)
            sanityLoss += extraSanityLossLowHealth;

        sanity -= sanityLoss * dt;

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

        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        sanity = Mathf.Clamp(sanity, 0, maxSanity);
        hunger = Mathf.Clamp(hunger, 0, maxHunger);

        if (health <= 0)
            Respawn();

        if (sanity <= 0)
            DeathBySanity();
    }

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
        health = maxHealth * 0.5f; 
        stamina = maxStamina;
    }

    void DeathBySanity()
    {
        movement.canMove = false;

        SceneManager.LoadScene("DeleteScene");

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
