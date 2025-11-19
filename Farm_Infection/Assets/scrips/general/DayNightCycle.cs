using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance { get; private set; }

    public float dayDuration = 120f; // 2 minutos
    public float nightDuration = 240f; // 4 minutos

    [Header("Luz del sol (opcional)")]
    public Light sunLight;
    public float dayLightIntensity = 1f;
    public float nightLightIntensity = 0.1f;

    public bool IsNight { get; private set; }

    public event Action OnDayStarted;
    public event Action OnNightStarted;

    private float timer;
    private bool firstFrame = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Empezamos de día
        IsNight = false;
        timer = 0f;
        ApplyLighting();
        OnDayStarted?.Invoke();
    }

    private void Update()
    {
        float duration = IsNight ? nightDuration : dayDuration;
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            timer = 0f;
            IsNight = !IsNight;
            ApplyLighting();

            if (IsNight)
                OnNightStarted?.Invoke();
            else
                OnDayStarted?.Invoke();
        }

        RotateSun(duration);
    }

    void ApplyLighting()
    {
        if (sunLight == null) return;

        sunLight.intensity = IsNight ? nightLightIntensity : dayLightIntensity;
    }

    void RotateSun(float currentPhaseDuration)
    {
        if (sunLight == null) return;

        // 0..1 dentro de la fase actual
        float t = Mathf.Clamp01(timer / currentPhaseDuration);

        // Giro simple: día 0–180°, noche 180–360°
        float baseAngle = IsNight ? 180f : 0f;
        float targetAngle = baseAngle + 180f * t;

        Vector3 euler = sunLight.transform.rotation.eulerAngles;
        euler.x = targetAngle;
        sunLight.transform.rotation = Quaternion.Euler(euler);
    }
}
