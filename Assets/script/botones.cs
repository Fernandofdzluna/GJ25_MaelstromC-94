using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class botones : MonoBehaviour
{
    // Empty Object para opciones
    public GameObject optionsMenu;

    // Otro Empty Object a activar/desactivar
    public GameObject alternateMenu;
    public Slider sliderB, sliderV;
    public float sliderValueB, sliderValueV;
    public Image panelBrillo, imagenMute;

    // Nombre de la escena que se quiere cargar
    public string sceneName;

    public Toggle pantallaCOmpleta;
    public TMP_Dropdown dropdown;
    public Resolution[] resoluciones;

    public AudioClip boton;

    // Método para cambiar de escena

    public void Start()
    {
        sliderB.value = PlayerPrefs.GetFloat("brillo", 0.5f);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, sliderValueB);
        if(Screen.fullScreen)
        {
            pantallaCOmpleta.isOn = true;
        }
        else
        {
            pantallaCOmpleta.isOn = false;
        }
        RevisarResolucion();
        sliderV.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = sliderV.value;
        RevisarMute();
    }

    public void RevisarMute()
    {
        if(sliderValueV == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }
    public void ChangeSlider2(float valor)
    {
        sliderValueV = valor;
        PlayerPrefs.SetFloat("volumen", sliderValueV);
        AudioListener.volume = sliderV.value;
        RevisarMute();
    }

    public void ChangeSlider(float valor)
    {
        sliderValueB = valor;
        PlayerPrefs.SetFloat("brillo", sliderValueB);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, sliderB.value);
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SoundFXManager.instance.PlaySoundFXCLip(boton, transform, 1f);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("El nombre de la escena no está asignado.");
        }
    }

    // Método para activar/desactivar el menú de opciones y alternar con alternateMenu
    public void ToggleOptionsMenu()
    {
        if (optionsMenu != null && alternateMenu != null)
        {
            bool isOptionsActive = optionsMenu.activeSelf;
            SoundFXManager.instance.PlaySoundFXCLip(boton, transform, 1f);
            optionsMenu.SetActive(!isOptionsActive);
            alternateMenu.SetActive(isOptionsActive); // Ocurre en viceversa
        }
        else
        {
            Debug.LogError("Los objetos 'optionsMenu' o 'alternateMenu' no están asignados en el inspector.");
        }
    }

    // Método para salir de la aplicación
    public void ExitApplication()
    {
        Debug.Log("Saliendo de la aplicación...");
        SoundFXManager.instance.PlaySoundFXCLip(boton, transform, 1f);
        Application.Quit();
    }

    public void PantallaCOmpletaCheck(bool pantallaCompletita)
    {
        Screen.fullScreen = pantallaCompletita;
    }

    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        dropdown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for(int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + "x" + resoluciones[i].height;
            opciones.Add(opcion);
            if(Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }
        dropdown.AddOptions(opciones);
        dropdown.value = resolucionActual;
        dropdown.RefreshShownValue();

        dropdown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        PlayerPrefs.SetInt("numeroResolucion", dropdown.value);

        Resolution resolution = resoluciones[indiceResolucion];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
