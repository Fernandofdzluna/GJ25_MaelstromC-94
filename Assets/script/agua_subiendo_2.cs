using UnityEngine;

public class agua_subiendo_2 : MonoBehaviour
{
    [Tooltip("Tiempo que deseas que tarde en alcanzar la altura máxima (segundos).")]
    public float timeToMaxHeight = 300f;

    [Tooltip("Tiempo de inicio de la elevación en segundos.")]
    public float startDelay = 0f;

    [Tooltip("Altura máxima que el plano alcanzará.")]
    public float maxHeight = 10f;

    private float elevationSpeed; // Velocidad calculada automáticamente
    private float elapsedTime = 0f; // Tiempo acumulado desde que empezó la elevación
    private bool isElevating = false; // Indica si el plano debe empezar a elevarse
    private Vector3 initialPosition; // Posición inicial del plano

    void Start()
    {
        // Calcular la velocidad de elevación basada en el tiempo deseado y la altura máxima
        elevationSpeed = maxHeight / timeToMaxHeight;

        // Guardar la posición inicial del plano
        initialPosition = transform.position;

        // Iniciar el temporizador tras el retraso configurado
        StartCoroutine(StartElevationAfterDelay());
    }

    void Update()
    {
        if (isElevating && transform.position.y < initialPosition.y + maxHeight)
        {
            // Elevar el plano gradualmente basándose en la velocidad de elevación
            transform.position += Vector3.up * elevationSpeed * Time.deltaTime;

            // Incrementar el tiempo acumulado
            elapsedTime += Time.deltaTime;

            // Clampear la altura para no exceder la máxima
            float clampedY = Mathf.Min(transform.position.y, initialPosition.y + maxHeight);
            transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
        }
    }

    private System.Collections.IEnumerator StartElevationAfterDelay()
    {
        // Esperar el tiempo de retraso antes de comenzar a elevar
        yield return new WaitForSeconds(startDelay);
        isElevating = true;
    }

    public void ResetElevation()
    {
        // Restablecer la posición del plano a la inicial
        transform.position = initialPosition;
        elapsedTime = 0f;
        isElevating = false;

        // Reiniciar el temporizador tras el retraso configurado
        StartCoroutine(StartElevationAfterDelay());
    }
}
