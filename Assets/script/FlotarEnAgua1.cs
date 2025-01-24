using UnityEngine;

public class FlotarEnAgua1 : MonoBehaviour
{
    public Transform referenciaAltura; // Objeto de referencia para la altura
    public float waterDensity = 0.5f;
    public float waterDrag = 5.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        if (referenciaAltura != null)
        {
            // Actualizar el nivel del agua según la posición Y del objeto de referencia
            float waterLevel = referenciaAltura.position.y;

            if (transform.position.y < waterLevel)
            {
                rb.linearDamping = waterDrag;
                rb.angularDamping = waterDrag;
                Vector3 waterUp = new Vector3(0.0f, 1.0f, 0.0f);
                float displacementFactor = Mathf.Clamp01((waterLevel - transform.position.y) / GetComponent<Renderer>().bounds.extents.y);
                float upForce = displacementFactor * waterDensity * Mathf.Abs(Physics.gravity.y) * GetComponent<Rigidbody>().mass;
                Vector3 appliedForce = upForce * waterUp;
                rb.AddForce(appliedForce, ForceMode.Force);
            }
            else
            {
                rb.linearDamping = 0.0f;
                rb.angularDamping = 0.05f;
            }
        }
        else
        {
            Debug.LogWarning("Referencia de altura no asignada.");
        }
    }
}
