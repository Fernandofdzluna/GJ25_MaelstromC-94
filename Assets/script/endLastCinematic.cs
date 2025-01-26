using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLastCinematic : MonoBehaviour
{
    public AudioClip uno, dos, tres, cuatro, cinco;

    public void ChangeScene()
    {
        StartCoroutine(waitScene());
    }

    public void PistaAudio1()
    {
        SoundFXManager.instance.PlaySoundFXCLip(uno, transform, 1f);
    }
    public void PistaAudio2()
    {
        SoundFXManager.instance.PlaySoundFXCLip(dos, transform, 0.7f);
    }
    public void PistaAudio3()
    {
        SoundFXManager.instance.PlaySoundFXCLip(tres, transform, 0.9f);
    }

    public void PistaAudio4()
    {
        SoundFXManager.instance.PlaySoundFXCLip(cuatro, transform, 1f);
    }
    public void PistaAudio5()
    {
        SoundFXManager.instance.PlaySoundFXCLip(cinco, transform, 1f);
    }

    IEnumerator waitScene()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("creditos");
    }
}
