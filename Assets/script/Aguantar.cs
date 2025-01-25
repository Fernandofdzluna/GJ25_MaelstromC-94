using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Aguantar : MonoBehaviour
{
    public GameObject globalWater;  // El objeto que activa el contador
    public Camera mainCamera;       // La cámara principal
    public Volume globalVolume;     // El Global Volume en la escena
    public bool isEffectActive = false;     // El booleano que se activa al terminar el contador

    public float shakeIntensity = 0.1f;  // Intensidad de la sacudida de la cámara
    public float initialShakeIntensity = 0.1f;  // Intensidad inicial de la sacudida
    public float initialSaturation = 0f;    // Saturación inicial
    public float initialVignette = 0.45f;  // Intensidad inicial de la viñeta

    private ColorAdjustments colorAdjustments;  // Para ajustar la saturación
    private Vignette vignette;                // Para ajustar el efecto de viñeta

    public float finalSaturation = -100f;   // Saturación final
    public float finalVignette = 1f;       // Intensidad final de la viñeta

    public float countdownTime = 20f;   // Tiempo total del contador (en segundos)
    public float shakeTimer;             // Tiempo restante para la sacudida de la cámara

    public PostWater1 otherScript;  // Referencia al otro script que controla el inicio del efecto

    private void Start()
    {
        // Obtener los efectos del Global Volume
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.saturation.overrideState = true;
            colorAdjustments.saturation.value = initialSaturation;
        }

        if (globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = initialVignette;
        }

        shakeTimer = countdownTime;  // Iniciar el contador
    }

    private void Update()
    {
        // Verificar si el otro script ha activado el booleano startEffect
        if (isEffectActive == true)
        {
            // Reducir el contador con el tiempo
            shakeTimer -= Time.deltaTime;

            // Si el contador aún tiene tiempo, aplicar efectos
            if (shakeTimer > 0)
            {
                ApplyWithout();
            }
            else
            {
                // Al finalizar el contador, activar el booleano
                isEffectActive = false;
            }
        }
    }

    private void ApplyCameraShake(float intensity)
    {
        
    }

    public void ApplyWithout()
    {
        // Calcular la intensidad de la sacudida de la cámara
        //float shakeFactor = Mathf.Lerp(initialShakeIntensity, shakeIntensity, (countdownTime - shakeTimer) / countdownTime);
        //ApplyCameraShake(shakeFactor);

        // Ajustar la saturación y viñeta a medida que el contador baja
        float saturation = Mathf.Lerp(initialSaturation, finalSaturation, (countdownTime - shakeTimer) / countdownTime);
        colorAdjustments.saturation.value = saturation;

        float vignetteIntensity = Mathf.Lerp(initialVignette, finalVignette, (countdownTime - shakeTimer) / countdownTime);
        vignette.intensity.value = vignetteIntensity;

        isEffectActive = true;
    }
}
