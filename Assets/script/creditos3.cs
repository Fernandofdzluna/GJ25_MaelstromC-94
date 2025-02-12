using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting.FullSerializer.Internal;

public class creditos3 : MonoBehaviour
{
    public GameObject[] creditContainers;
    Transform[] spawnPoints;
    bool[] spawnsSet;
    public float moveSpeed = 5f;
    public float maxY = 10f;
    public float displayTime = 2f;
    public float buttonTextDelay = 1f; // Nuevo tiempo de espera antes de mostrar el texto
    public float horizontalSpeed = 1f;
    public float horizontalRange = 2f;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timerText;
    public float gameDuration = 30f;
    public string nextSceneName;
    private int totalPoints = 0;
    private float remainingTime;
    public AudioSource audioSource;
    public Canvas canvas;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        remainingTime = gameDuration;

        spawnPoints = new Transform[creditContainers.Length];
        spawnsSet = new bool[creditContainers.Length];
        for (int i = 0; i < creditContainers.Length; i++)
        {
            spawnPoints[i] = creditContainers[i].transform;
            spawnsSet[i] = false;
        }

        foreach (var container in creditContainers)
        {
            Button button = container.GetComponentInChildren<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnButtonClicked(container, button));
            }
            SetRandomSpawnPoint(container); // Asigna un punto de inicio aleatorio
        }

        UpdatePointsText();
        UpdateTimerText();
        StartCoroutine(GameTimer());
    }

    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        foreach (var container in creditContainers)
        {
            if (container.activeSelf)
            {
                container.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                float sinValue = Mathf.Sin(Time.time * horizontalSpeed + container.GetInstanceID() * 0.1f);
                container.transform.Translate(Vector3.right * sinValue * horizontalRange * Time.deltaTime);

                if (container.transform.position.y >= maxY)
                {
                    //SetRandomSpawnPoint(container);
                }
            }
        }
    }

    void OnButtonClicked(GameObject container, Button button)
    {
        TextMeshProUGUI text = container.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            button.gameObject.SetActive(false);
            audioSource.Play();
            //SpawnParticleEffect();
            StartCoroutine(ShowButtonAfterDelay(button));
        }
        AddPoints(100);
        //SetRandomSpawnPoint(container); // Mueve el contenedor a un nuevo punto aleatorio al hacer clic
    }

    IEnumerator ShowButtonAfterDelay(Button button)
    {
        yield return new WaitForSeconds(displayTime + buttonTextDelay); // Agrega la espera adicional
        if (button != null)
        {
            /*
            button.gameObject.SetActive(true);
            button.transform.parent.GetChild(0).gameObject.SetActive(false);
            */
            button.gameObject.SetActive(true);
            GameObject parentObject = button.gameObject.transform.parent.gameObject;
            parentObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void SetRandomSpawnPoint(GameObject container)
    {
        int randomNum = Random.Range(0, spawnPoints.Length);
        Debug.Log(randomNum);
        if (spawnsSet[randomNum])
        {
            container.transform.position = spawnPoints[randomNum].position;
        }
        else
        {
            SetRandomSpawnPoint(container);
        }
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

        foreach (var container in creditContainers)
        {
            container.SetActive(false);
        }
        timerText.gameObject.SetActive(false);
        pointsText.text = "Puntuación Final: " + totalPoints.ToString();

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nextSceneName);
    }
}
