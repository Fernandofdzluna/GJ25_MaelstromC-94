using UnityEngine;

public class agua_subiendo_2 : MonoBehaviour
{
    [Tooltip("Tiempo que deseas que tarde en alcanzar la altura m�xima (segundos).")]
    public float timeToMaxHeight = 300f;

    [Tooltip("Tiempo de inicio de la elevaci�n en segundos.")]
    public float startDelay = 0f;

    [Tooltip("Altura m�xima que el plano alcanzar�.")]
    public float maxHeight = 10f;

    private float elevationSpeed; // Velocidad calculada autom�ticamente
    private float elapsedTime = 0f; // Tiempo acumulado desde que empez� la elevaci�n
    private bool isElevating = false; // Indica si el plano debe empezar a elevarse
    private Vector3 initialPosition; // Posici�n inicial del plano

    void Start()
    {
        // Calcular la velocidad de elevaci�n basada en el tiempo deseado y la altura m�xima
        elevationSpeed = maxHeight / timeToMaxHeight;

        // Guardar la posici�n inicial del plano
        initialPosition = transform.position;

        // Iniciar el temporizador tras el retraso configurado
        StartCoroutine(StartElevationAfterDelay());
    }

    void Update()
    {
        if (isElevating && transform.position.y < initialPosition.y + maxHeight)
        {
            // Elevar el plano gradualmente bas�ndose en la velocidad de elevaci�n
            transform.position += Vector3.up * elevationSpeed * Time.deltaTime;

            // Incrementar el tiempo acumulado
            elapsedTime += Time.deltaTime;

            // Clampear la altura para no exceder la m�xima
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
        // Restablecer la posici�n del plano a la inicial
        transform.position = initialPosition;
        elapsedTime = 0f;
        isElevating = false;

        // Reiniciar el temporizador tras el retraso configurado
        StartCoroutine(StartElevationAfterDelay());
    }
}
