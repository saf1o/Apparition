using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10f;
    public float rotationSmoothTime = 0.2f;
    
    private float _yVelocity;
    private Vector3 _currentRotation;
    
    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _currentRotation = transform.eulerAngles;
    }

    void Update()
    {
        Movement();
        CameraRotation();
    }

    private void Movement()
    {
        // Playerの前後左右移動
        float xMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float zMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(xMovement, 0, zMovement);
        
        // マウスカーソルで視点移動
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        
        // ｘ方向に一定量移動していれば横回転
        if (Mathf.Abs(mx) > 0.001f)
        {
            transform.RotateAround(transform.position, Vector3.up, mx);
        }
    }

    private void CameraRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        moveDirection = _cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;
        transform.position += moveDirection * speed * Time.deltaTime;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(_currentRotation.y, targetAngle, ref _yVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        _currentRotation.y = transform.eulerAngles.y;
    }
}
