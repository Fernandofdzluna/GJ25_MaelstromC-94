using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Empty Object para opciones
    public GameObject optionsMenu;

    // Otro Empty Object a activar/desactivar
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

    // M�todo para activar/desactivar el men� de opciones y alternar con alternateMenu
    public void ToggleOptionsMenu()
    {
        if (optionsMenu != null && alternateMenu != null)
        {
            bool isOptionsActive = optionsMenu.activeSelf;
            optionsMenu.SetActive(!isOptionsActive);
            alternateMenu.SetActive(isOptionsActive); // Ocurre en viceversa
        }
        else
        {
            Debug.LogError("Los objetos 'optionsMenu' o 'alternateMenu' no est�n asignados en el inspector.");
        }
    }

    // M�todo para salir de la aplicaci�n
    public void ExitApplication()
    {
        Debug.Log("Saliendo de la aplicaci�n...");
        Application.Quit();
    }
}
