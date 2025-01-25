using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour
{
    public Image imagenAMostrar; // Referencia a la imagen que quieres mostrar
    //public Aguantar1 scriptCount; // Referencia al script Count
    public Aguantar scriptCount; // Referencia al script Count

    public float tiempoTransicion = 1f; // Tiempo en segundos que dura la transición
    private float tiempoTranscurrido = 0f;

    private void Update()
    {
        // Verificar si el shake timer en el script Count ha llegado a 1
        if (scriptCount.shakeTimer <= 1)
        {
            // Si la imagen no es nula, iniciar la transición
            if (imagenAMostrar != null)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = Mathf.Clamp01(tiempoTranscurrido / tiempoTransicion); // Valor entre 0 y 1 para la interpolación
                imagenAMostrar.color = Color.Lerp(Color.clear, Color.black, t);
            }
        }
    }
}
