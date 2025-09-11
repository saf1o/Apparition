using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float y_sensitivity = 3f;
    [SerializeField] private float x_sensitivity = 3f;
    public float y_mouse;
    public float x_mouse;
    public float CamMAX_x;
    public float CamMIN_x;
    public float CamMAX_y;
    public float CamMIN_y;

    public MoveController Player;
    public GameObject kay_text_set;
    public GameObject target;
    
    void FixedUpdate()
    {
        // マウスの動き
        y_mouse = Input.GetAxis("Mouse Y");
        x_mouse = Input.GetAxis("Mouse X");

        Vector3 newRotation = transform.localEulerAngles;
        
        // カメラの回転量
        newRotation.x -= y_mouse * y_sensitivity;
        newRotation.y += x_mouse * x_sensitivity;
        
        // カメラ回転上限
        if (CamMIN_x > newRotation.x)
        {
            if (newRotation.x > 180) newRotation.x = CamMIN_x;
        }

        if (newRotation.x > CamMAX_x)
        {
            if (180 > newRotation.x) newRotation.x = CamMAX_x;
        }

        if (CamMIN_y > newRotation.y)
        {
            if (newRotation.y > 180) newRotation.y = CamMIN_y;
        }

        if (newRotation.y > CamMAX_y)
        {
            if (180 > newRotation.y) newRotation.y = CamMAX_y;
        }
        transform.localEulerAngles = newRotation;
        
        RaycastHit hit;
    
        if (Physics.SphereCast(gameObject.transform.position, 0.1f,
                target.transform.forward, out hit, 5f))
        {
            if (hit.collider.gameObject.tag == "Kay")
            {
                kay_text_set.SetActive(true);
                if (Input.GetMouseButton(0))
                {
                    Player.Kay += 1;
                    Destroy(hit.collider.gameObject);
                    kay_text_set.SetActive(false);
                }
            }
            else kay_text_set.SetActive(false);
        }
        Debug.DrawRay(gameObject.transform.position, target.transform.forward * 10, Color.blue);
    }
}
