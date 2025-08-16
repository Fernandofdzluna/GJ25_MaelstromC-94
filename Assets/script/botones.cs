using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class botones : MonoBehaviour
{
    // Empty Object para opciones
    public GameObject optionsMenu, pausaMenu;
    //public static bool enPausa = false;

    // Otro Empty Object a activar/desactivar
    public GameObject alternateMenu, panelConfirmacion;
    public Slider sliderB, sliderV;
    public float sliderValueB, sliderValueV;
    public Image panelBrillo, imagenMute;

    // Nombre de la escena que se quiere cargar
    public string sceneName;

    public Toggle pantallaCOmpleta;
    public TMP_Dropdown dropdown;
    public Resolution[] resoluciones;

    public AudioClip boton;

    [Header("Lista de oraciones")]
    public string[] oraciones;  // Aquí agregas todas las frases posibles
    public TextMeshProUGUI textoUI;  // Arrastrar el TextMeshProUGUI desde el inspector


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
        //RevisarMute();
    }

    public void ChangeSlider2(float valor)
    {
        sliderValueV = valor;
        PlayerPrefs.SetFloat("volumen", sliderValueV);
        AudioListener.volume = sliderV.value;
        //RevisarMute();
        if (sliderValueV == 0)
        {
            imagenMute.enabled = true;
        }
        else
        {
            imagenMute.enabled = false;
        }
    }

    public void ChangeSlider()
    {
        sliderB.onValueChanged.AddListener(AjustarBrillo);
        sliderB.value = 1f; // Brillo inicial máximo
        AjustarBrillo(sliderB.value);
    }

    // Método que ajusta el brillo
    public void AjustarBrillo(float valor)
    {
        if (panelBrillo == null) return;

        // El valor del slider va de 0 (oscuro) a 1 (brillo máximo)
        // Ajustamos la alpha del overlay: alpha = 1 - valor
        Color c = panelBrillo.color;
        c.a = 1f - valor;
        panelBrillo.color = c;
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

    public void ConfirmarSalir()
    {
        SoundFXManager.instance.PlaySoundFXCLip(boton, transform, 1f);
        // Detectamos si el objeto se desactiva
        if (!panelConfirmacion.activeInHierarchy)
        {
            CambiarOracion();
            panelConfirmacion.SetActive(true); // Lo volvemos a activar si quieres que se repita varias veces
        }
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

    public void ContinuarJugando()
    {
        optionsMenu.SetActive(false);
        pausaMenu.SetActive(false);
        Time.timeScale = 1f;
        ActivarCursor(false);
    }

    public void PausarJuego()
    {
        //optionsMenu.SetActive(true);
        pausaMenu.SetActive(true);
        Time.timeScale = 0f;
        ActivarCursor(true);
    }

    private void ActivarCursor(bool activado)
    {
        Cursor.visible = activado; // Mostrar u ocultar cursor
        Cursor.lockState = activado ? CursorLockMode.None : CursorLockMode.Locked; // Bloquear o liberar
    }

    public void VolverAlMenuInicial()
    {
        SoundFXManager.instance.PlaySoundFXCLip(boton, transform, 1f);
        SceneManager.LoadScene("inicio");
    }

    public void CancelarSalir()
    {
        SoundFXManager.instance.PlaySoundFXCLip(boton, transform, 1f);
        panelConfirmacion.SetActive(false);
    }

    private void CambiarOracion()
    {
        if (oraciones.Length == 0) return;

        // Elegir una oración aleatoria
        int indice = Random.Range(0, oraciones.Length);
        string oracionSeleccionada = oraciones[indice];

        // Mostrarla en TextMeshPro
        textoUI.text = oracionSeleccionada;
    }
}
