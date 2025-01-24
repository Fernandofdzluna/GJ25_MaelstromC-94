using UnityEngine;
using System.Collections;

public class lucesEmergencia : MonoBehaviour
{
    public Light emergencyLight; // Referencia a la luz que se modificará
    public Color color1 = Color.red; // Primer color (por defecto rojo)
    public Color color2 = Color.blue; // Segundo color (por defecto azul)
    public float changeInterval = 0.5f; // Intervalo de cambio en segundos

    private bool isColor1 = true; // Variable para alternar entre colores

    void Start()
    {
        if (emergencyLight == null)
        {
            emergencyLight = GetComponent<Light>();
            if (emergencyLight == null)
            {
                Debug.LogError("No se encontró una luz. Asigna una luz al script.");
                return;
            }
        }

        // Inicia la rutina de cambio de color
        StartCoroutine(ChangeLightColor());
    }

    private IEnumerator ChangeLightColor()
    {
        while (true) // Bucle infinito para el cambio continuo
        {
            // Cambia el color de la luz
            emergencyLight.color = isColor1 ? color1 : color2;

            // Alterna entre color1 y color2
            isColor1 = !isColor1;

            // Espera el tiempo definido antes de cambiar
            yield return new WaitForSeconds(changeInterval);
        }
    }
}
