using UnityEngine;

public class PlayerPOV : MonoBehaviour
{
    public Transform neck;
    public float sensitivity;
    public float minVertical;
    public float maxVertical;
    
    private float rotationX;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        
        transform.Rotate(0, mouseX, 0);
    }
}
