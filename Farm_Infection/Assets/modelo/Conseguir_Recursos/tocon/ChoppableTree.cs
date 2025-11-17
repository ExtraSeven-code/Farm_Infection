using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class ChoppableTree : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Herramienta necesaria")]
    public ToolType requiredTool = ToolType.Axe;

    [Header("Drop")]
    public GameObject dropPrefab;  // 👈 prefab del WorldItemDrop
    public ItemData dropItem;      // Item que va dentro del drop
    public int dropAmount = 3;     // cuánta madera suelta

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Hit(ItemData tool)
    {
        if (tool == null || !tool.isTool)
        {
            Debug.Log("Necesitas una herramienta para talar este árbol.");
            return;
        }

        if (tool.toolType != requiredTool)
        {
            Debug.Log("Esta herramienta no sirve. Necesitas un hacha.");
            return;
        }

        currentHealth -= tool.toolPower;
        Debug.Log("Golpeaste el tronco. Vida: " + currentHealth);

        if (currentHealth <= 0)
            ChopDown();
    }

    void ChopDown()
    {
        // ✅ En vez de Inventory.AddItem → spawneamos drops
        if (dropPrefab != null && dropItem != null)
        {
            // Puedes soltar varios ítems
            int remaining = dropAmount;

            while (remaining > 0)
            {
                int stack = Mathf.Min(remaining, 1); // si quieres 1 por drop, deja 1

                Vector3 spawnPos = transform.position + Vector3.up +
                                   Random.insideUnitSphere * 0.3f;

                GameObject dropObj = Instantiate(dropPrefab, spawnPos, Quaternion.identity);

                WorldItemDrop drop = dropObj.GetComponent<WorldItemDrop>();
                if (drop != null)
                {
                    drop.item = dropItem;
                    drop.amount = stack;
                }

                remaining -= stack;
            }
        }

        // Efectos opcionales: partículas, sonido, cambiar a tocón, etc.
        Destroy(gameObject);
    }
}
