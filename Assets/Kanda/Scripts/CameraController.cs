using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float y_sensitivity = 3f;
    [SerializeField] private float x_sensitivity = 3f;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private float minLookAngle = -20f;
    
    private float rotationX = 0f;
    
    public float y_mouse;
    public float x_mouse;

    public MoveController Player;
    public GameObject kay_text_set;
    public GameObject target;

    [Header("Player Synchronization")] 
    public Transform playerTransform;
    void Update()
    {
        // マウスの動き
        y_mouse = Input.GetAxis("Mouse Y");
        x_mouse = Input.GetAxis("Mouse X");
        
        if (playerTransform != null)
            playerTransform.Rotate(0, x_mouse * x_sensitivity, 0);
        
        // カメラの回転量
        rotationX -= y_mouse * y_sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);
        
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        
        RaycastHit hit;
        
        // カメラ回転上限
        if (Physics.SphereCast(transform.position, 0.1f,
                transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Kay"))
            {
                kay_text_set.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    Player.Kay += 1;
                    Destroy(hit.collider.gameObject);
                    kay_text_set.SetActive(false);
                }
            }
            else kay_text_set.SetActive(false);
        }
        else kay_text_set.SetActive(false);
        
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
    }

    public void ResetCameraRotation()
    {
        rotationX = 0;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        Debug.Log("CameraController: 回転リセット");
    }
}
