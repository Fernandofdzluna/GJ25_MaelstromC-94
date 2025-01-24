using UnityEngine;

public class objectPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("objectPosition", new Vector4(this.transform.position.x, this.transform.position.y, this.transform.position.z, this.transform.localScale.x));
    }
}
