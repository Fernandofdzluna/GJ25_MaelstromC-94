using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using StarterAssets;

public class EndGame : MonoBehaviour
{
    public Image imagenAMostrar; // Referencia a la imagen que quieres mostrar
    public Aguantar scriptCount; // Referencia al script Count
    FirstPersonController firstPersonController;

    public float tiempoTransicion = 1f; // Tiempo en segundos que dura la transición
    private float tiempoTranscurrido = 0f;

    bool final = false;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = player.GetComponent<FirstPersonController>();
    }

    public void EndTheGame()
    {
        if (final == false)
        {
            // Si la imagen no es nula, iniciar la transición
            if (imagenAMostrar.gameObject.activeInHierarchy)
            {
                tiempoTranscurrido += Time.deltaTime;
                //float t = Mathf.Clamp01(tiempoTranscurrido / tiempoTransicion); // Valor entre 0 y 1 para la interpolación
                float t = 0.5f * Time.deltaTime;
                imagenAMostrar.color = Color.Lerp(Color.clear, Color.black, t);
                StartCoroutine(fadeIn(true));
            }
            final = true;
        }
    }

    IEnumerator fadeIn(bool tipoFade)
    {
        var tempColor = imagenAMostrar.color;

        if (tipoFade)
        {
            for (int i = 0; i < 50; i++)
            {
                tempColor.a += 0.02f;
                yield return new WaitForSeconds(0.1f);
                imagenAMostrar.color = tempColor;
                if(firstPersonController.ahogandose == false)
                {
                    StartCoroutine(fadeIn(false));
                    scriptCount.ApplyWithout(false);
                    yield break;
                }
            }
            firstPersonController.DeathPlayer();
        }
        else
        {
            for (int i = 0; i< 25; i++)
            {
                tempColor.a -= 0.04f;
                yield return new WaitForSeconds(0.2f);
                imagenAMostrar.color = tempColor;
            }
            final = false;
        }
    }
}
