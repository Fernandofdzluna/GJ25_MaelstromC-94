using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLastCinematic : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("creditos");
    }
}
