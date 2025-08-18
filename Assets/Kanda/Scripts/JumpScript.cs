using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4.0f;
    Rigidbody rb;
    
    private string groundTag = "Ground";

    private bool isJumping;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isJumping = false;
            Debug.Log("Jumping");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == groundTag)
        {
            isJumping = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == groundTag)
        {
            isJumping = false;
        }
    }
}
