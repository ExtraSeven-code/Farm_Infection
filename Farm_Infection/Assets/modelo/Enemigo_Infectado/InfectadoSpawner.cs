using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectadoSpawner : MonoBehaviour
{
    public GameObject infectedPrefab;
    public Transform spawnPoint;

    private GameObject currentInfected;
    private bool subscribed = false;

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void Update()
    {
        if (!subscribed)
            TrySubscribe();
    }

    private void OnDisable()
    {
        if (DayNightCycle.Instance != null && subscribed)
        {
            DayNightCycle.Instance.OnNightStarted -= SpawnInfected;
            DayNightCycle.Instance.OnDayStarted -= DespawnInfected;
        }
        subscribed = false;
    }

    void TrySubscribe()
    {
        if (DayNightCycle.Instance == null) return;
        if (subscribed) return;

        DayNightCycle.Instance.OnNightStarted += SpawnInfected;
        DayNightCycle.Instance.OnDayStarted += DespawnInfected;
        subscribed = true;

        Debug.Log("[Spawner] Suscrito a DayNightCycle");

        if (DayNightCycle.Instance.IsNight)
        {
            SpawnInfected();
        }
    }

    void SpawnInfected()
    {
        if (infectedPrefab == null)
        {
            Debug.LogWarning("[Spawner] infectedPrefab NO asignado");
            return;
        }
        if (currentInfected != null)
        {
            Debug.Log("[Spawner] Ya hay un infectado activo, no spawneo otro.");
            return;
        }

        Vector3 pos = spawnPoint ? spawnPoint.position : transform.position;
        Quaternion rot = spawnPoint ? spawnPoint.rotation : transform.rotation;

        currentInfected = Instantiate(infectedPrefab, pos, rot);
        Debug.Log("[Spawner] Infectado spawneado en " + pos);
    }

    void DespawnInfected()
    {
        if (currentInfected != null)
        {
            Debug.Log("[Spawner] Despawning infectado");
            Destroy(currentInfected);
            currentInfected = null;
        }
    }
}
