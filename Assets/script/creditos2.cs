using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class creditos2 : MonoBehaviour
{
    public List<GameObject> creditContainers;
    public float moveSpeed = 5f;
    public float maxY = 10f;
    public float displayTime = 2f;
    public float horizontalSpeed = 1f;
    public float horizontalRange = 2f;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timerText;
    public float gameDuration = 30f;
    public string nextSceneName; // Nombre de la escena a la que cambiar
    private int totalPoints = 0;
    private float remainingTime;
    public AudioSource audioSource;
    public ParticleSystem particleEffect;
    public Canvas canvas; // Asigna el Canvas en el Inspector

    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        /*
        if (GUI.Button(new Rect(125, 0, 100, 50), "Lock Cursor"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        */
        Cursor.lockState = CursorLockMode.None;
        // Obtener el componente AudioSource del GameObject
        audioSource = GetComponent<AudioSource>();
        remainingTime = gameDuration;
        foreach (var container in creditContainers)
        {
            Button button = container.GetComponentInChildren<Button>();
            if (button != null)
            {
                initialPositions[container] = container.transform.position;
                button.onClick.AddListener(() => OnButtonClicked(container, button));
            }
            container.transform.position = new Vector3(
                container.transform.position.x + Random.Range(-horizontalRange, horizontalRange),
                container.transform.position.y,
                container.transform.position.z
            );
        }

        UpdatePointsText();
        UpdateTimerText();
        StartCoroutine(GameTimer());
    }

    void Update()
    {
        foreach (var container in creditContainers)
        {
            if (container.activeSelf)
            {
                container.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                float sinValue = Mathf.Sin(Time.time * horizontalSpeed + container.GetInstanceID() * 0.1f);
                container.transform.Translate(Vector3.right * sinValue * horizontalRange * Time.deltaTime);

                if (container.transform.position.y >= maxY)
                {
                    ResetContainer(container);
                }
            }
        }
    }
    public void SpawnParticleEffect()
    {
        // Obtener la posición del clic en pantalla
        Vector2 mousePosition = Input.mousePosition;

        // Convertir la posición de pantalla a coordenadas del mundo
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out Vector3 worldPosition
        );

        // Mover el sistema de partículas a la posición calculada
        particleEffect.transform.position = worldPosition;

        // Reproducir el sistema de partículas
        particleEffect.Play();
    }

    void OnButtonClicked(GameObject container, Button button)
    {
        TextMeshProUGUI text = container.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            button.gameObject.SetActive(false);
            audioSource.Play();
            SpawnParticleEffect();
            StartCoroutine(ShowButtonAfterDelay(button));
        }
        AddPoints(100);
    }

    IEnumerator ShowButtonAfterDelay(Button button)
    {
        yield return new WaitForSeconds(displayTime);
        if (button != null)
        {
            button.gameObject.SetActive(true);
            GameObject parentObject = button.gameObject.transform.parent.gameObject;
            parentObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void ResetContainer(GameObject container)
    {
        container.transform.position = initialPositions[container];
        container.transform.position = new Vector3(
            container.transform.position.x + Random.Range(-horizontalRange, horizontalRange),
            container.transform.position.y,
            container.transform.position.z
        );
    }

    void AddPoints(int points)
    {
        totalPoints += points;
        UpdatePointsText();
    }

    void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "Puntos: " + totalPoints.ToString();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Tiempo: " + Mathf.CeilToInt(remainingTime).ToString();
        }
    }

    IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
            UpdateTimerText();
        }

        // Ocultar todos los botones y mostrar solo la puntuación final
        foreach (var container in creditContainers)
        {
            container.SetActive(false);
        }
        timerText.gameObject.SetActive(false); // Ocultar el temporizador
        pointsText.text = "Puntuación Final: " + totalPoints.ToString();

        // Esperar 3 segundos antes de cambiar de escena
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nextSceneName);
    }
}