using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class creditos : MonoBehaviour
{
    public Button[] developerButtons; // Asigna los botones en el inspector
    public TextMeshProUGUI scoreText; // Asigna el texto para mostrar el puntaje usando TMP
    public TextMeshProUGUI timerText; // Texto para mostrar el tiempo restante
    public float riseSpeed = 2f; // Velocidad a la que las burbujas suben
    public float sideSpeed = 0.5f; // Velocidad lateral
    public float maxHeight = 10f; // Altura máxima antes de reiniciar
    public float bubbleRadius = 0.5f; // Radio de la burbuja para la interacción lateral
    public float delayBeforeStart = 1f; // Retraso de 1 segundo
    public float textDisplayTime = 2f; // Tiempo de visualización del texto
    public float gameDuration = 60f; // Duración del juego en segundos
    public List<string> customTexts = new List<string> { "Texto 1", "Texto 2", "Texto 3" }; // Lista de textos a mostrar

    private int score = 0;
    private float remainingTime;

    void Start()
    {
        score = 0;
        remainingTime = gameDuration;
        UpdateScoreText();
        UpdateTimerText();
        foreach (Button button in developerButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
        Invoke("StartMovingBubbles", delayBeforeStart);
        StartCoroutine(GameTimer());
    }

    void StartMovingBubbles()
    {
        foreach (Button button in developerButtons)
        {
            StartCoroutine(MoveBubble(button.gameObject));
        }
    }

    void OnButtonClick(Button button)
    {
        // Sumar puntos
        score += 100;
        UpdateScoreText();

        // Desactivar el botón y mostrar texto
        StartCoroutine(ShowTextAndReset(button.gameObject));
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();
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
        // Aquí puedes añadir lógica para finalizar el juego, como desactivar botones, mostrar puntuación final, etc.
        foreach (Button button in developerButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowTextAndReset(GameObject bubble)
    {
        // Posición donde se mostró el botón
        Vector3 bubblePosition = bubble.transform.position;

        // Desactivar el botón
        bubble.SetActive(false);

        // Crear texto temporal en la misma posición usando TextMeshPro
        GameObject textObject = new GameObject("TempText");
        textObject.transform.position = bubblePosition;
        TextMeshProUGUI tmpText = textObject.AddComponent<TextMeshProUGUI>();
        tmpText.color = Color.white; // Establecer el color del texto en blanco
        int textIndex = 0;
        while (textIndex < customTexts.Count && remainingTime > 0)
        {
            string currentText = customTexts[textIndex];
            tmpText.text = currentText;
            Debug.Log(currentText); // Escribir el texto en la consola
            tmpText.fontSize = 20;
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.rectTransform.sizeDelta = new Vector2(150, 50);

            // Esperar el tiempo especificado o hasta que el juego termine
            float waitTime = Mathf.Min(textDisplayTime, remainingTime);
            yield return new WaitForSeconds(waitTime);

            textIndex++; // Pasar al siguiente texto
            if (textIndex < customTexts.Count)
            {
                // Si hay más textos, limpiar el texto actual antes de mostrar el siguiente
                tmpText.text = "";
            }
        }

        // Eliminar el texto temporal
        Destroy(textObject);

        // Reiniciar la burbuja solo si el juego aún no ha terminado
        if (remainingTime > 0)
        {
            bubble.transform.position = bubblePosition;
            bubble.SetActive(true);
            StartCoroutine(MoveBubble(bubble));
        }
    }

    IEnumerator MoveBubble(GameObject bubble)
    {
        Vector3 startPos = bubble.transform.position;
        Vector3 currentPos = startPos;

        while (remainingTime > 0)
        {
            // Movimiento hacia arriba
            currentPos.y += Time.deltaTime * riseSpeed;

            // Movimiento lateral aleatorio
            float sideMovement = Mathf.Sin(Time.time * sideSpeed) * bubbleRadius;
            currentPos.x = startPos.x + sideMovement;

            bubble.transform.position = currentPos;

            // Reiniciar posición si la burbuja ha llegado a la altura máxima
            if (currentPos.y >= maxHeight)
            {
                currentPos = startPos;
            }

            yield return null;
        }
    }
}