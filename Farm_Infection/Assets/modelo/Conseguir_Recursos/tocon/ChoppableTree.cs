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
    public GameObject dropPrefab;  
    public ItemData dropItem;      
    public int dropAmount = 3;     

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
        if (dropPrefab != null && dropItem != null)
        {
            int remaining = dropAmount;

            while (remaining > 0)
            {
                int stack = Mathf.Min(remaining, 1); 

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

        Destroy(gameObject);
    }
}
