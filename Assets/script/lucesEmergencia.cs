using UnityEngine;
using System.Collections;

public class lucesEmergencia : MonoBehaviour
{
    // Variables configurables desde el Inspector

    public Light emergencyLight; // Referencia a la luz
    public float maxIntensity = 5f; // Intensidad máxima de la luz
    public float minIntensity = 0f; // Intensidad mínima de la luz
    public float pulseSpeed = 2f; // Velocidad del parpadeo

    private bool isIncreasing = true; // Controla si la intensidad está aumentando o disminuyendo

    void Start()
    {
        // Si no se asigna la luz en el Inspector, se usa la luz del objeto al que está asignado el script
        if (emergencyLight == null)
        {
            emergencyLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (emergencyLight != null)
        {
            // Aumentar o disminuir la intensidad según el estado
            if (isIncreasing)
            {
                emergencyLight.intensity += pulseSpeed * Time.deltaTime;
                if (emergencyLight.intensity >= maxIntensity)
                {
                    emergencyLight.intensity = maxIntensity;
                    isIncreasing = false; // Cambiar de aumentar a disminuir
                }
            }
            else
            {
                emergencyLight.intensity -= pulseSpeed * Time.deltaTime;
                if (emergencyLight.intensity <= minIntensity)
                {
                    emergencyLight.intensity = minIntensity;
                    isIncreasing = true; // Cambiar de disminuir a aumentar
                }
            }
        }
    }
}
