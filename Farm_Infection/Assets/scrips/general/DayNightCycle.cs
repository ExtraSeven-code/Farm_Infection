using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    [Header("Luz del sol")]
    public Light sunLight;

    [Header("Duraciones (segundos)")]
    public float dayDuration = 120f;   // 2 minutos
    public float nightDuration = 240f; // 4 minutos

    [Header("Colores / Intensidad (opcional)")]
    public Gradient sunColorOverDay;          // opcional
    public AnimationCurve sunIntensityOverDay; // opcional (0..1)

    public bool IsNight { get; private set; }

    private float cycleLength;  // day + night
    private float cycleTime;    // 0..cycleLength

    void Start()
    {
        cycleLength = dayDuration + nightDuration;

        if (sunLight == null)
            sunLight = FindObjectOfType<Light>();
    }

    void Update()
    {
        if (sunLight == null) return;

        // avanzar tiempo de ciclo (0..cycleLength)
        cycleTime += Time.deltaTime;
        if (cycleTime > cycleLength)
            cycleTime -= cycleLength;

        // t global 0..1 en el ciclo completo
        float t = cycleTime / cycleLength;

        // ROTACIÓN CONTINUA 0 → 360, sin saltos
        float angle = t * 360f - 90f; // -90º para que el amanecer quede más bonito
        sunLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        // Día/Noche (día es el primer tramo del ciclo)
        float dayPortion = dayDuration / cycleLength;
        IsNight = t > dayPortion;

        // Color opcional
        if (sunColorOverDay != null && sunColorOverDay.colorKeys.Length > 0)
            sunLight.color = sunColorOverDay.Evaluate(t);

        // Intensidad opcional (por ejemplo: 1 de día, 0 de noche)
        if (sunIntensityOverDay != null && sunIntensityOverDay.keys.Length > 0)
        {
            float intensityFactor = sunIntensityOverDay.Evaluate(t);
            sunLight.intensity = intensityFactor;
        }
    }
}
