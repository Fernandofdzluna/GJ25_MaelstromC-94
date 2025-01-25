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

    // M�todo para cambiar de escena
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("El nombre de la escena no est� asignado.");
        }
    }

    // M�todo para activar/desactivar el men� de opciones
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
            Debug.LogError("El objeto 'optionsMenu' no est� asignado en el inspector.");
        }
    }

    // M�todo para salir de la aplicaci�n
    public void ExitApplication()
    {
        Debug.Log("Saliendo de la aplicaci�n...");
        Application.Quit();
    }
}
