using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;
    public CharacterController controller;
    public Animator animator;
    
    private Vector3 velocity;
    private bool isGrounded;
    
    void Update()
    {
        if (Input.GetKeyDown("Space") && isGrounded)
        {
             velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        IsGround();
    }
    
    private void IsGround()
    {
        isGrounded =  controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = transform.TransformDirection(new Vector3(h,0,v)) * moveSpeed;
        Vector3 playervelocity = moveDirection * moveSpeed;
        
        bool isMoving = playervelocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
        
        velocity.y += gravity * Time.deltaTime;
        
        controller.Move((moveDirection + velocity) * Time.deltaTime);
    }
}
