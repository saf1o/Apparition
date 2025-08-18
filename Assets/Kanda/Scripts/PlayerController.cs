using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Wキーで前進
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
            Debug.Log("W");
        }
        
        // Aキーで左方向
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * (speed * Time.deltaTime);
            Debug.Log("A");
        }
        
        // Sキーで後退
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * (speed * Time.deltaTime);
            Debug.Log("S");
        }
        
        // Dキーで右方向
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * (speed * Time.deltaTime);
            Debug.Log("D");
        }
    }
}
