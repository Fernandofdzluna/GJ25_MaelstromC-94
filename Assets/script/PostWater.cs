using UnityEngine;

public class PostWater : MonoBehaviour
{
    public GameObject waterGlobalVolume; // Asigna aqu� el objeto con el componente Volume.

    private void Start()
    {
        if (waterGlobalVolume != null)
        {
            // Aseg�rate de que el volumen est� desactivado inicialmente.
            waterGlobalVolume.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Capsula");
        // Aseg�rate de verificar si el objeto que entra es el jugador.
        if (other.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(true); // Activa el volumen.
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Capsula");
        // Aseg�rate de verificar si el objeto que entra es el jugador.
        if (collision.collider.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(true); // Activa el volumen.
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Saliendo");
        // Aseg�rate de verificar si el objeto que sale es el jugador.
        if (other.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(false); // Desactiva el volumen.
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("Saliendo");
        // Aseg�rate de verificar si el objeto que sale es el jugador.
        if (collision.collider.CompareTag("Player"))
        {
            waterGlobalVolume.SetActive(false); // Desactiva el volumen.
        }
    }
}
