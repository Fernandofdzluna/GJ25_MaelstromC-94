using StarterAssets;
using UnityEngine;

public class PostWater : MonoBehaviour
{
    FirstPersonController script_FristPersonController;
    public GameObject waterGlobalVolume; // Asigna aquí el objeto con el componente Volume.
    public Transform movingObject; // Asigna aquí el transform del objeto que se mueve constantemente.
    public Transform player; // Asigna aquí el transform del jugador.
    public bool cintura = false; // Booleano para indicar si el jugador está a la altura de la cintura.

    private float playerHeight;
    private float waistHeight;
    private float thresholdHeight;

    private void Awake()
    {
        script_FristPersonController = player.gameObject.GetComponent<FirstPersonController>();
    }

    private void Start()
    {
        if (waterGlobalVolume != null)
        {
            // Asegúrate de que el volumen esté desactivado inicialmente.
            waterGlobalVolume.SetActive(false);
        }

        if (player != null)
        {
            // Obtén la altura del jugador desde el CharacterController.
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                playerHeight = characterController.height; // Altura total del CharacterController.
                waistHeight = player.position.y + (playerHeight / 2); // Altura de la cintura (mitad del cuerpo).
                thresholdHeight = player.position.y + (5 * playerHeight / 6); // Altura de 5/6 del cuerpo.
            }
            else
            {
                Debug.LogError("El jugador no tiene un CharacterController asignado.");
            }
        }
        else
        {
            Debug.LogError("El transform del jugador no está asignado.");
        }
    }

    private void Update()
    {
        if (movingObject != null && waterGlobalVolume != null)
        {
            float movingObjectY = movingObject.position.y;

            // Activa el volumen si el objeto que sube alcanza la altura de 5/6 o más.
            if (movingObjectY >= thresholdHeight)
            {
                if (!waterGlobalVolume.activeSelf)
                {
                    waterGlobalVolume.SetActive(true);
                    Debug.Log("Volumen activado: objeto alcanzó 5/6 de la altura.");
                    script_FristPersonController.MoveSpeed = 1;
                }
            }
            else
            {
                if (waterGlobalVolume.activeSelf)
                {
                    waterGlobalVolume.SetActive(false);
                    Debug.Log("Volumen desactivado: objeto por debajo de 5/6 de la altura.");
                    script_FristPersonController.MoveSpeed = 4;
                }
            }
        }

        if (player != null)
        {
            float playerY = player.position.y;

            // Activa el booleano "cintura" si el jugador está a la altura de la cintura o más abajo.
            if (playerY <= waistHeight)
            {
                if (!cintura)
                {
                    cintura = true;
                    Debug.Log("Booleano cintura activado.");
                    script_FristPersonController.MoveSpeed = 2;
                }
            }
            else
            {
                if (cintura)
                {
                    cintura = false;
                    Debug.Log("Booleano cintura desactivado.");
                    script_FristPersonController.MoveSpeed = 4;
                }
            }
        }
    }
}
