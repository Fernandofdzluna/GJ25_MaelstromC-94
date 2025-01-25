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
        // Obtener la posici�n del rat�n en pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convertir la posici�n del rat�n al espacio del Canvas (si es necesario)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent.GetComponent<RectTransform>(),
            mousePosition,
            null, // C�mara utilizada (null usa la c�mara principal)
            out Vector2 localPoint
        );

        // Calcular distancia al rat�n
        float distanceToMouse = Mathf.Abs(localPoint.y - rectTransform.anchoredPosition.y) + extra;

        // Interpolar suavemente el tama�o
        float newHeight = Mathf.Lerp(rectTransform.sizeDelta.y, distanceToMouse, Time.deltaTime * smoothSpeed);

        // Actualizar el tama�o
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

    }
}
