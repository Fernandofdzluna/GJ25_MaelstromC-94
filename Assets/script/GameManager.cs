using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    GameObject player;
    Transform spawnPoint;
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
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
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

        player.transform.position = spawnPoint.position;
        player.GetComponent<StarterAssetsInputs>().enabled = false;
        player.GetComponent<FirstPersonController>().enabled = false;
        StartCoroutine(wakeUpAnimationFinished());
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
            GameObject visorTutoBombonas = GameObject.Find("RegistroBombonas");
            visorTutoBombonas.SetActive(false);
            GameObject padreCanvas = visorTutoBombonas.transform.parent.gameObject;
            padreCanvas.transform.GetChild(2).gameObject.SetActive(true);
            EndGame(true);
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

    public void EndGame(bool gameCompleted)
    {
        if (gameCompleted)
        {
            lucesEmergencia[] lights = FindObjectsByType<lucesEmergencia>(FindObjectsSortMode.None);
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].emergencyLight.color = Color.green;
            }
        }
        else
        {
            StartCoroutine(finishGame());
        }
    }

    IEnumerator finishGame()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Fin");
        //SceneManager.LoadScene("");
    }

    IEnumerator wakeUpAnimationFinished()
    {
        yield return new WaitForSeconds(13);
        player.GetComponent<StarterAssetsInputs>().enabled = true;
        player.GetComponent<FirstPersonController>().enabled = true;
        Destroy(player.transform.GetChild(1).gameObject);
        player.GetComponent<Animator>().enabled = false;
    }
}
