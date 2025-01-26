using UnityEngine;

public class bombonasVFX : MonoBehaviour
{
    GameObject bombonaVFX;
    GameObject aguaScene;
    bool bajoAgua;

    private void Awake()
    {
        bombonaVFX = this.gameObject.transform.GetChild(0).gameObject;
        bombonaVFX.SetActive(false);
        aguaScene = GameObject.FindGameObjectWithTag("agua");
        bajoAgua = false;
    }

    private void LateUpdate()
    {
        if (bajoAgua == false)
        {
            if (bombonaVFX.transform.position.y < aguaScene.transform.position.y)
            {
                Debug.Log("Bombona por debajo de agua");
                changeVFXState(true);
                bajoAgua = true;
            }
        }
        else
        {
            if (bombonaVFX.transform.position.y >= aguaScene.transform.position.y)
            {
                Debug.Log("Bombona por encima de agua");
                changeVFXState(false);
                bajoAgua = false;
            }
        }
    }

    void changeVFXState(bool isActive)
    {
        bombonaVFX.SetActive (isActive);
    }
}
