using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    Rigidbody rb;

    void Start()
    {
        rb =  GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Wキーで前進
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * speed;
            Debug.Log("W");
        }
        
        // Aキーで左方向
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = - transform.right * speed;
            Debug.Log("A");
        }
        
        // Sキーで後退
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = - transform.forward * speed;
            Debug.Log("S");
        }
        
        // Dキーで右方向
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = transform.right * speed;
            Debug.Log("D");
        }
    }
}
