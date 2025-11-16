using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
    [Header("Dónde aparece la planta")]
    public Transform plantSpawnPoint;

    [Header("Crecimiento")]
    public float growthTime = 20f;
    public bool requiresWater = true;

    private GameObject plantInstance;
    private float growthTimer;
    private bool hasSeed;
    private bool isFullyGrown;
    private bool isWatered;
    private ItemData plantedSeed;

    private void Update()
    {
        if (!hasSeed || isFullyGrown)
            return;

        if (requiresWater && !isWatered)
            return;

        growthTimer += Time.deltaTime;
        float t = Mathf.Clamp01(growthTimer / growthTime);

        if (plantInstance != null)
        {
            float scale = Mathf.Lerp(0.3f, 1f, t);
            plantInstance.transform.localScale = Vector3.one * scale;
        }

        if (t >= 1f)
        {
            isFullyGrown = true;
        }
    }

    public bool CanPlant()
    {
        return !hasSeed;
    }

    public bool TryPlant(ItemData seed)
    {
        if (hasSeed || seed == null || !seed.isSeed || seed.plantPrefab == null)
            return false;

        hasSeed = true;
        isFullyGrown = false;
        isWatered = false;
        growthTimer = 0f;
        plantedSeed = seed;

        Vector3 pos = plantSpawnPoint != null ? plantSpawnPoint.position : transform.position;

        plantInstance = Instantiate(seed.plantPrefab, pos, Quaternion.identity, transform);
        plantInstance.transform.localScale = Vector3.one * 0.3f;

        return true;
    }

    public void Water()
    {
        isWatered = true;
        // Aquí luego puedes cambiar color tierra, partículas, etc.
    }

    public bool TryHarvest(out ItemData fruit, out int amount)
    {
        fruit = null;
        amount = 0;

        if (!isFullyGrown || plantedSeed == null)
            return false;

        fruit = plantedSeed.harvestItem;
        amount = plantedSeed.harvestAmount > 0 ? plantedSeed.harvestAmount : 1;

        if (plantInstance != null)
            Destroy(plantInstance);

        hasSeed = false;
        isFullyGrown = false;
        isWatered = false;
        growthTimer = 0f;
        plantedSeed = null;

        return fruit != null;
    }
}
