using System.Collections;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLastCinematic : MonoBehaviour
{
    public void ChangeScene()
    {
        StartCoroutine(waitScene());
    }

    IEnumerator waitScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("creditos");
    }
}
