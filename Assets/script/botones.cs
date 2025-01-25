using UnityEngine;
using UnityEngine.SceneManagement;

public class botones : MonoBehaviour
{
    // Empty Object para opciones
    public GameObject optionsMenu;

    // Otro Empty Object a activar cuando se desactiva optionsMenu
    public GameObject alternateMenu;

    // Nombre de la escena que se quiere cargar
    public string sceneName;

    // Método para cambiar de escena
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("El nombre de la escena no está asignado.");
        }
    }

    // Método para activar/desactivar el menú de opciones
    public void ToggleOptionsMenu()
    {
        if (optionsMenu != null)
        {
            bool isActive = optionsMenu.activeSelf;
            optionsMenu.SetActive(!isActive);

            if (alternateMenu != null)
            {
                alternateMenu.SetActive(isActive); // Activar alternateMenu si optionsMenu se desactiva
            }
        }
        else
        {
            Debug.LogError("El objeto 'optionsMenu' no está asignado en el inspector.");
        }
    }

    // Método para salir de la aplicación
    public void ExitApplication()
    {
        Debug.Log("Saliendo de la aplicación...");
        Application.Quit();
    }
}
