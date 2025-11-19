using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Recurso a spawnear")]
    public GameObject resourcePrefab;

    [Header("Tiempo entre respawns (segundos)")]
    public float respawnTime = 240f; // 4 minutos

    [Header("Opcional: punto exacto de spawn")]
    public Transform spawnPoint;

    private GameObject currentResource;
    private float timer;

    private void Start()
    {
        SpawnResource();
    }

    private void Update()
    {
        if (currentResource != null) return;

        // Si el recurso ya no existe, empezamos a contar
        timer += Time.deltaTime;

        if (timer >= respawnTime)
        {
            SpawnResource();
            timer = 0;
        }
    }

    void SpawnResource()
    {
        if (resourcePrefab == null) return;

        Vector3 pos = spawnPoint ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;

        currentResource = Instantiate(resourcePrefab, pos, rot);
    }
}
