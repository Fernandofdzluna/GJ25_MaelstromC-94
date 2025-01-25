using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class Aguantar1 : MonoBehaviour
{
    public GameObject globalWater;  // El objeto que activa el contador
    public Camera mainCamera;       // La cámara principal
    public Volume globalVolume;     // El Global Volume en la escena
    public bool isEffectActive;     // El booleano que se activa al terminar el contador

    public float shakeIntensity = 0.1f;  // Intensidad de la sacudida de la cámara
    public float initialShakeIntensity = 0.1f;  // Intensidad inicial de la sacudida

    public Vector4 initialLift = new Vector4(1f, 1f, 1f, 0f); // Valor inicial de Lift
    public Vector4 initialGamma = new Vector4(1f, 1f, 1f, 0f); // Valor inicial de Gamma
    public Vector4 initialGain = new Vector4(1f, 1f, 1f, 0f); // Valor inicial de Gain
    public float initialSaturation = 0f;    // Saturación inicial
    public float initialVignette = 0.45f;  // Intensidad inicial de la viñeta
    public float initialLensDistortion = 0f; // Intensidad inicial de Lens Distortion

    private LiftGammaGain liftGammaGain; // Controlar Lift Gamma Gain
    private ColorAdjustments colorAdjustments;  // Para ajustar la saturación
    private Vignette vignette;                // Para ajustar el efecto de viñeta
    private LensDistortion lensDistortion;    // Para ajustar Lens Distortion

    public Vector4 targetLift = new Vector4(0.5f, 0.5f, 0.5f, 0f); // Lift final
    public Vector4 targetGamma = new Vector4(0.8f, 0.8f, 0.8f, 0f); // Gamma final
    public Vector4 targetGain = new Vector4(1.2f, 1.2f, 1.2f, 0f); // Gain final
    public float finalSaturation = -100f;   // Saturación final
    public float finalVignette = 1f;       // Intensidad final de la viñeta
    public float finalLensDistortion = -0.5f; // Intensidad final de Lens Distortion

    public float countdownTime = 20f;   // Tiempo total del contador (en segundos)
    public float shakeTimer;             // Tiempo restante para la sacudida de la cámara

    public float smoothTransitionTime = 2f; // Tiempo de transición suave general
    public float liftGammaTime = 2f;        // Tiempo de transición para Lift Gamma Gain
    public float lensDistortionTime = 3f;   // Tiempo de transición para Lens Distortion

    public PostWater1 otherScript;  // Referencia al otro script que controla el inicio del efecto


    private void Start()
    {
        // Obtener los efectos del Global Volume
        if (globalVolume.profile.TryGet(out liftGammaGain))
        {
            liftGammaGain.lift.overrideState = true;
            liftGammaGain.gamma.overrideState = true;
            liftGammaGain.gain.overrideState = true;

            liftGammaGain.lift.value = initialLift;
            liftGammaGain.gamma.value = initialGamma;
            liftGammaGain.gain.value = initialGain;
        }

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

        if (globalVolume.profile.TryGet(out lensDistortion))
        {
            lensDistortion.intensity.overrideState = true;
            lensDistortion.intensity.value = initialLensDistortion;
        }

        shakeTimer = countdownTime;  // Iniciar el contador
    }

    private void Update()
    {
        // Verificar si el otro script ha activado el booleano startEffect
        if (otherScript != null && otherScript.ojos)
        {
            // Reducir el contador con el tiempo
            shakeTimer -= Time.deltaTime;

            // Si el contador aún tiene tiempo, aplicar efectos
            if (shakeTimer > 0)
            {
                // Calcular progreso de interpolación
                float progress = (countdownTime - shakeTimer) / countdownTime;

                // Aplicar todos los efectos juntos
                StartCoroutine(ApplyCombinedEffects(progress));
            }
            else
            {
                // Al finalizar el contador, activar el booleano
                isEffectActive = true;
                StartCoroutine(SmoothDeactivateEffects()); // Desactivar suavemente
            }
        }
    }

    private IEnumerator ApplyCombinedEffects(float progress)
    {
        float elapsedTimeLift = 0f;
        float elapsedTimeLens = 0f;

        Vector4 targetLiftValue = Vector4.Lerp(initialLift, targetLift, progress);
        Vector4 targetGammaValue = Vector4.Lerp(initialGamma, targetGamma, progress);
        Vector4 targetGainValue = Vector4.Lerp(initialGain, targetGain, progress);
        float targetSaturation = Mathf.Lerp(initialSaturation, finalSaturation, progress);
        float targetVignette = Mathf.Lerp(initialVignette, finalVignette, progress);
        float targetLensDistortion = Mathf.Lerp(initialLensDistortion, finalLensDistortion, progress);

        while (elapsedTimeLift < liftGammaTime || elapsedTimeLens < lensDistortionTime)
        {
            if (elapsedTimeLift < liftGammaTime)
            {
                elapsedTimeLift += Time.deltaTime;
                float tLift = Mathf.Clamp01(elapsedTimeLift / liftGammaTime);

                // Interpolar valores de Lift Gamma Gain
                liftGammaGain.lift.value = Vector4.Lerp(liftGammaGain.lift.value, targetLiftValue, tLift);
                liftGammaGain.gamma.value = Vector4.Lerp(liftGammaGain.gamma.value, targetGammaValue, tLift);
                liftGammaGain.gain.value = Vector4.Lerp(liftGammaGain.gain.value, targetGainValue, tLift);
            }

            if (elapsedTimeLens < lensDistortionTime)
            {
                elapsedTimeLens += Time.deltaTime;
                float tLens = Mathf.Clamp01(elapsedTimeLens / lensDistortionTime);

                // Interpolar valores de Lens Distortion
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, targetLensDistortion, tLens);
            }

            // Aplicar Saturación y Viñeta independientemente
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, targetSaturation, Time.deltaTime / smoothTransitionTime);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetVignette, Time.deltaTime / smoothTransitionTime);

            yield return null;
        }
    }

    private IEnumerator SmoothDeactivateEffects()
    {
        float elapsedTimeLift = 0f;
        float elapsedTimeLens = 0f;

        Vector4 currentLift = liftGammaGain.lift.value;
        Vector4 currentGamma = liftGammaGain.gamma.value;
        Vector4 currentGain = liftGammaGain.gain.value;
        float currentSaturation = colorAdjustments.saturation.value;
        float currentVignette = vignette.intensity.value;
        float currentLensDistortion = lensDistortion.intensity.value;

        while (elapsedTimeLift < liftGammaTime || elapsedTimeLens < lensDistortionTime)
        {
            if (elapsedTimeLift < liftGammaTime)
            {
                elapsedTimeLift += Time.deltaTime;
                float tLift = Mathf.Clamp01(elapsedTimeLift / liftGammaTime);

                // Interpolar valores hacia el inicial de forma suave
                liftGammaGain.lift.value = Vector4.Lerp(currentLift, initialLift, tLift);
                liftGammaGain.gamma.value = Vector4.Lerp(currentGamma, initialGamma, tLift);
                liftGammaGain.gain.value = Vector4.Lerp(currentGain, initialGain, tLift);
            }

            if (elapsedTimeLens < lensDistortionTime)
            {
                elapsedTimeLens += Time.deltaTime;
                float tLens = Mathf.Clamp01(elapsedTimeLens / lensDistortionTime);

                // Interpolar Lens Distortion hacia su valor inicial
                lensDistortion.intensity.value = Mathf.Lerp(currentLensDistortion, initialLensDistortion, tLens);
            }

            // Aplicar Saturación y Viñeta independientemente
            colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, initialSaturation, Time.deltaTime / smoothTransitionTime);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, initialVignette, Time.deltaTime / smoothTransitionTime);

            yield return null;
        }
    }
}