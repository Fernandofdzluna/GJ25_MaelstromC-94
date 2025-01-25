using UnityEngine;

public class StretchToMouse : MonoBehaviour
{
    private RectTransform rectTransform;
    public float smoothSpeed = 5f; // Velocidad de suavizado
    public float extra; // Velocidad de suavizado

    void Start()
    {
        // Obtener el RectTransform del objeto
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Obtener la posición del ratón en pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posición del ratón al espacio del Canvas (si es necesario)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(),
            mousePosition,
            null, // Cámara utilizada (null usa la cámara principal)
            out Vector2 localPoint
        );

        // Calcular distancia al ratón
        float distanceToMouse = Mathf.Abs(localPoint.y - rectTransform.anchoredPosition.y) + extra;

        // Interpolar suavemente el tamaño
        float newHeight = Mathf.Lerp(rectTransform.sizeDelta.y, distanceToMouse, Time.deltaTime * smoothSpeed);

        // Actualizar el tamaño
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

    }
}
