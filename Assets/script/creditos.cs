using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class creditos : MonoBehaviour
{
    public Button[] developerButtons; // Asigna los botones en el inspector
    public Text scoreText; // Asigna el texto para mostrar el puntaje
    public float riseSpeed = 2f; // Velocidad a la que las burbujas suben
    public float sideSpeed = 0.5f; // Velocidad lateral
    public float maxHeight = 10f; // Altura m�xima antes de reiniciar
    public float bubbleRadius = 0.5f; // Radio de la burbuja para la interacci�n lateral
    public float delayBeforeStart = 1f; // Retraso de 1 segundo
    public float textDisplayTime = 2f; // Tiempo de visualizaci�n del texto

    private int score = 0;

    void Start()
    {
        score = 0;
        UpdateScoreText();
        foreach (Button button in developerButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
        Invoke("StartMovingBubbles", delayBeforeStart);
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

        // Desactivar el bot�n y mostrar texto
        StartCoroutine(ShowTextAndReset(button.gameObject));
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    IEnumerator ShowTextAndReset(GameObject bubble)
    {
        // Posici�n donde se mostr� el bot�n
        Vector3 bubblePosition = bubble.transform.position;

        // Desactivar el bot�n
        bubble.SetActive(false);

        // Crear texto temporal en la misma posici�n
        GameObject textObject = new GameObject("TempText");
        textObject.transform.position = bubblePosition;
        Text textComponent = textObject.AddComponent<Text>();
        textComponent.text = "Aqu� va tu texto personalizado"; // Cambia esto al texto que desees
        textComponent.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textComponent.fontSize = 20;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.rectTransform.sizeDelta = new Vector2(150, 50); // Ajusta el tama�o seg�n necesites

        // Esperar 2 segundos
        yield return new WaitForSeconds(textDisplayTime);

        // Eliminar el texto temporal
        Destroy(textObject);

        // Reiniciar la burbuja
        bubble.transform.position = bubblePosition; // Asegurarse de que vuelve a la posici�n inicial
        bubble.SetActive(true);
        StartCoroutine(MoveBubble(bubble));
    }

    IEnumerator MoveBubble(GameObject bubble)
    {
        Vector3 startPos = bubble.transform.position;
        Vector3 currentPos = startPos;

        while (true)
        {
            // Movimiento hacia arriba
            currentPos.y += Time.deltaTime * riseSpeed;

            // Movimiento lateral aleatorio
            float sideMovement = Mathf.Sin(Time.time * sideSpeed) * bubbleRadius;
            currentPos.x = startPos.x + sideMovement;

            bubble.transform.position = currentPos;

            // Reiniciar posici�n si la burbuja ha llegado a la altura m�xima
            if (currentPos.y >= maxHeight)
            {
                currentPos = startPos;
            }

            yield return null;
        }
    }
}