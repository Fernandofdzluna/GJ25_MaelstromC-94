using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLastCinematic : MonoBehaviour
{
    public void ChangeScene()
    {
        StartCoroutine(waitScene());
    }

    public void PistaAudio1()
    {

    }
    public void PistaAudio2()
    {

    }
    public void PistaAudio3()
    {

    }

    public void PistaAudio4()
    {

    }

    IEnumerator waitScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("creditos");
    }
}
