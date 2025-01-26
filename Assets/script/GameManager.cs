using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject aguaAscendente;
    agua_subiendo_2 scriptAguaAscendente;
    float unidadSubidaAgua;

    public int BombonasARecoger;
    public int BombonasPickedUp;
    public TextMeshProUGUI textoBombonasMax;
    GameObject[] submarinoPreVisitaScreens;
    GameObject[] submarinoPostSalasVistas;

    RectTransform barraAguaTransform;
    float aguaMaxHeight;

    private void OnEnable()
    {
        UpgradeBombonasNumber(0);
        submarinoPreVisitaScreens = GameObject.FindGameObjectsWithTag("PreSalasBombonasVisitar");
        submarinoPostSalasVistas = GameObject.FindGameObjectsWithTag("SalaBombonasVisitada");
        for (int z = 0; z < submarinoPostSalasVistas.Length; z++)
        {
            submarinoPostSalasVistas[z].SetActive(false);
            barraAguaTransform = submarinoPostSalasVistas[z].transform.Find("BarraAgua").GetComponent<RectTransform>();
            aguaMaxHeight = barraAguaTransform.sizeDelta.y;
            barraAguaTransform.sizeDelta = new Vector2(barraAguaTransform.sizeDelta.x, 0);
        }
        scriptAguaAscendente = aguaAscendente.GetComponent<agua_subiendo_2>();
        unidadSubidaAgua = aguaMaxHeight / scriptAguaAscendente.timeToMaxHeight;
    }

    void Update()
    {
        for (int z = 0; z < submarinoPostSalasVistas.Length; z++)
        {
            barraAguaTransform = submarinoPostSalasVistas[z].transform.Find("BarraAgua").GetComponent<RectTransform>();
            barraAguaTransform.sizeDelta = new Vector2(barraAguaTransform.sizeDelta.x, barraAguaTransform.sizeDelta.y + (unidadSubidaAgua * Time.deltaTime));
        }
    }

    public void UpgradeBombonasNumber(int add)
    {
        BombonasPickedUp += add;
        textoBombonasMax.SetText(BombonasPickedUp + "/" + BombonasARecoger);
        if (BombonasPickedUp >= 18)
        {
            Debug.Log("Juego Completado");
        }
    }

    public void ChangeSubmarineScreens()
    {
        for (int i = 0; i < submarinoPreVisitaScreens.Length; i++)
        {
            submarinoPreVisitaScreens[i].SetActive(false);
        }
        for (int z = 0; z < submarinoPostSalasVistas.Length; z++)
        {
            submarinoPostSalasVistas[z].SetActive(true);
        }
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
