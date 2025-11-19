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
    [Tooltip("Cuánto dura el día en segundos")]
    public float dayDuration = 120f;     // 2 minutos

    [Tooltip("Cuánto dura la noche en segundos")]
    public float nightDuration = 240f;   // 4 minutos

    [Header("Rotación del sol")]
    [Tooltip("Offset en grados para que el sol salga donde tú quieras")]
    public float rotationOffset = -90f;  // -90 = amanecer cuando t ~ 0 (sol en el horizonte)

    [Header("Color del sol (opcional)")]
    [Tooltip("t va de 0..1 a lo largo de TODO el ciclo día+noche")]
    public Gradient sunColorOverDay;

    [Header("Intensidad del sol")]
    [Tooltip("Curva 0..1 a lo largo del ciclo;\n0 = noche, 1 = día completo")]
    public AnimationCurve sunIntensityOverDay;

    [Tooltip("Intensidad real máxima de día")]
    public float dayIntensity = 1.0f;

    [Tooltip("Intensidad mínima de noche (luz de luna)")]
    public float nightIntensity = 0.2f;

    // Estado público
    public bool IsNight { get; private set; }
    public float NormalizedTime01 => cycleTime / cycleLength;   // 0..1 en TODO el ciclo

    // Eventos (para que otros scripts se suscriban)
    public event Action OnDayStarted;
    public event Action OnNightStarted;

    // Internos
    private float cycleLength;   // day + night
    private float cycleTime;     // 0..cycleLength

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
        cycleLength = dayDuration + nightDuration;

        if (sunLight == null)
            sunLight = FindObjectOfType<Light>();

        // MUY IMPORTANTE: que la luz empiece “limpia”
        if (sunLight != null)
            sunLight.transform.rotation = Quaternion.identity;

        // Forzamos una primera actualización
        UpdateSunAndState(0f);
    }

    private void Update()
    {
        if (sunLight == null) return;

        // Avanzar tiempo
        cycleTime += Time.deltaTime;
        if (cycleTime > cycleLength)
            cycleTime -= cycleLength;

        UpdateSunAndState(Time.deltaTime);
    }

    private void UpdateSunAndState(float dt)
    {
        if (sunLight == null) return;

        float t = cycleTime / cycleLength;   // 0..1 en el ciclo completo

        // -------- ROTACIÓN CONTINUA --------
        // 0..1 → 0..360 grados (el sol da una vuelta entera)
        float angle = t * 360f + rotationOffset;
        sunLight.transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        // -------- DÍA / NOCHE (SEGÚN DURACIONES) --------
        float dayPortion = dayDuration / cycleLength;

        bool wasNight = IsNight;
        IsNight = t > dayPortion;   // primera parte del ciclo es día

        if (IsNight != wasNight)
        {
            if (IsNight) OnNightStarted?.Invoke();
            else OnDayStarted?.Invoke();
        }

        // -------- COLOR DEL SOL (OPCIONAL) --------
        if (sunColorOverDay != null && sunColorOverDay.colorKeys.Length > 0)
            sunLight.color = sunColorOverDay.Evaluate(t);

        // -------- INTENSIDAD SEGÚN ALTURA DEL SOL --------
        // La dirección hacia donde apunta el sol
        Vector3 dir = sunLight.transform.forward;

        // dot > 0 cuando el sol está por encima del “suelo”
        // dot = 1 cuando está justo encima (mediodía)
        float heightFactor = Mathf.Clamp01(Vector3.Dot(dir, Vector3.down));

        // Suavizamos el amanecer/atardecer
        heightFactor = Mathf.SmoothStep(0f, 1f, heightFactor);

        float factor = heightFactor;

        // Si además tienes una curva, la multiplicamos
        if (sunIntensityOverDay != null && sunIntensityOverDay.keys.Length > 0)
        {
            float curveFactor = Mathf.Clamp01(sunIntensityOverDay.Evaluate(t));
            factor *= curveFactor;
        }

        // Mezclamos entre intensidad de noche y de día
        float targetIntensity = Mathf.Lerp(nightIntensity, dayIntensity, factor);
        sunLight.intensity = targetIntensity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
#endif
}
