using StarterAssets;
using System.Collections;
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

    FirstPersonController firstPerson_script;  // Referencia al otro script que controla el inicio del efecto
    EndGame endGame_Script;

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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        firstPerson_script = player.GetComponent<FirstPersonController>();
        endGame_Script = this.gameObject.GetComponent<EndGame>();
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
                StartCoroutine(VigneteIn(true));
            }
            else
            {
                // Al finalizar el contador, activar el booleano
                isEffectActive = false;
                if (isEffectActive == false)
                endGame_Script.EndTheGame();
            }
        }
    }

    public void ApplyWithout(bool bSubir)
    {
        if (bSubir)
        {
            // Ajustar la saturación y viñeta a medida que el contador baja
            //float saturation = Mathf.Lerp(initialSaturation, finalSaturation, (countdownTime - shakeTimer) / countdownTime);
            //colorAdjustments.saturation.value = saturation;

            //float vignetteIntensity = Mathf.Lerp(initialVignette, finalVignette, (countdownTime - shakeTimer) / countdownTime);
            //vignette.intensity.value = vignetteIntensity;

            //isEffectActive = true;
            StartCoroutine(VigneteIn(true));
        }
        else
        {
            StartCoroutine(VigneteIn(false));
        }
    }

    IEnumerator VigneteIn(bool bSubir)
    {
        if (bSubir)
        {
            for (int i = 0; i < 100; i++)
            {
                colorAdjustments.saturation.value += 0.01f;
                vignette.intensity.value += 0.01f;
                yield return new WaitForSeconds(0.2f);
                if (firstPerson_script.ahogandose == false)
                {
                }
            }
            firstPerson_script.DeathPlayer();
        }
        /*else
        {
            for (int i = 0; i < 25; i++)
            {
                tempColor.a -= 0.04f;
                yield return new WaitForSeconds(0.2f);
                imagenAMostrar.color = tempColor;
            }
            final = false;
        }*/
        yield return null;
    }
}
