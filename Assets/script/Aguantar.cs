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

    public float initialSaturation;    // Saturación inicial
    public float initialVignette;  // Intensidad inicial de la viñeta

    private ColorAdjustments colorAdjustments;  // Para ajustar la saturación
    private Vignette vignette;                // Para ajustar el efecto de viñeta

    public int countdownTime;   // Tiempo total del contador (en segundos)

    FirstPersonController firstPerson_script;  // Referencia al otro script que controla el inicio del efecto
    EndGame endGame_Script;

    private void Start()
    {
        // Obtener los efectos del Global Volume
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.saturation.overrideState = true;
            initialSaturation = colorAdjustments.saturation.value;
        }

        if (globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
            initialVignette = vignette.intensity.value;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        firstPerson_script = player.GetComponent<FirstPersonController>();
        endGame_Script = this.gameObject.GetComponent<EndGame>();
    }

    public void ApplyWithout(bool bSubir)
    {
        if (bSubir)
        {
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
            yield return new WaitForSeconds(countdownTime);
            for (int i = 0; i < 150; i++)
            {
                colorAdjustments.saturation.value -= 0.1f;
                vignette.intensity.value += 0.01f;
                yield return new WaitForSeconds(0.08f);
                if (firstPerson_script.ahogandose == false || firstPerson_script.hasMascaraRespiracion)
                {
                    StartCoroutine(VigneteIn(false));
                    yield break;
                }
            }
            endGame_Script.EndTheGame();
            firstPerson_script.DeathPlayer();
        }
        else
        {
            colorAdjustments.saturation.value = initialSaturation;
            vignette.intensity.value = initialVignette;
        }
        yield return null;
    }
}
