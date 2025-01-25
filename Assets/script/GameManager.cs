using UnityEngine;
using TMPro;

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
}
