using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int BombonasARecoger;
    public int BombonasPickedUp;
    public TextMeshProUGUI textoBombonasMax;

    private void OnEnable()
    {
        UpgradeBombonasNumber();
    }

    public void UpgradeBombonasNumber()
    {
        textoBombonasMax.SetText(BombonasPickedUp + "/" + BombonasARecoger);
    }

    public void EndGame()
    {
        StartCoroutine(finishGame());
    }

    IEnumerator finishGame()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Fin");
        //SceneManager.LoadScene("");
    }
}
