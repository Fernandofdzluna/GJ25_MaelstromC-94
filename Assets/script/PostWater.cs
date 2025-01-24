using UnityEngine;

public class PostWater : MonoBehaviour
{
    public GameObject waterGlobalVolume; // Asigna aquí el objeto con el componente Volume.

    private void Start()
    {
        if (waterGlobalVolume != null)
        {
            // Asegúrate de que el volumen esté desactivado inicialmente.
            waterGlobalVolume.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Capsula");
        // Asegúrate de verificar si el objeto que entra es el jugador.
        if (other.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(true); // Activa el volumen.
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Capsula");
        // Asegúrate de verificar si el objeto que entra es el jugador.
        if (collision.collider.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(true); // Activa el volumen.
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Saliendo");
        // Asegúrate de verificar si el objeto que sale es el jugador.
        if (other.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(false); // Desactiva el volumen.
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("Saliendo");
        // Asegúrate de verificar si el objeto que sale es el jugador.
        if (collision.collider.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(false); // Desactiva el volumen.
        }
    }
}
