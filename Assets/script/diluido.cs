using UnityEngine;

[ExecuteInEditMode]
public class PostProcessEffect : MonoBehaviour
{
    public Material postProcessMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (postProcessMaterial != null)
        {
            // Aplica el material de post-procesado
            Graphics.Blit(src, dest, postProcessMaterial);
        }
        else
        {
            // Pasa la imagen sin cambios
            Graphics.Blit(src, dest);
        }
    }
}