using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 2.0f;
    [SerializeField] private float verticalSpeed = 2.0f;

    public float stamina;
    public Slider stamina_slider;
    private Animator animator;
    private GameObject camera;
    private Vector3 moveDirection;
    private CharacterController controller;
    public bool GameOver;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        KeySystem();
        StaminaBer();
        DontDestroyOnLoad();
        // Jump();
    }

    private void KeySystem()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Walk", true);
            
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position += transform.forward * 0.02f;
                stamina = stamina - (Time.deltaTime * 10);
            }
            else transform.position += transform.forward * 0.01f;
        }
        else animator.SetBool("Walk", false);
        
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Back", true);
            transform.position -= transform.forward * (speed * Time.deltaTime);
        }
        else animator.SetBool("Back", false);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            transform.Rotate(0.0f, -1.0f, 0.0f);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
            transform.Rotate(0.0f, 1.0f, 0.0f);

        if (Input.GetKey(KeyCode.Space)) animator.SetBool("Run", true);
        
        else animator.SetBool("Run", false);
    }
    
    private void StaminaBer()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            stamina = stamina +  (Time.deltaTime * 2);

        const float MIN = 0;
        const float MAX = 100;
        stamina = Mathf.Clamp(stamina, MIN, MAX);
        
        stamina_slider.value = stamina;
    }

    void DontDestroyOnLoad()
    {
        if (GameOver == true)
            SceneManager.LoadScene("GameOver");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
            GameOver = true;
    }
    // private void Jump()
    // {
    //     if (controller.isGrounded)
    //         if (Input.GetKeyDown("Jump")) moveDirection.y = jumpSpeed;
    //     
    //     moveDirection.y -= gravity * Time.deltaTime;
    //     controller.Move(moveDirection * Time.deltaTime);
    // }
}
