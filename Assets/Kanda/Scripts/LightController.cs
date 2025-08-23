using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject targetObject;
    
    void Update()
    {
        this.transform.LookAt(targetObject.transform);
    }
}
