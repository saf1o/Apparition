using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Walk", true);
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Back", true);
            transform.position -= transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Back", false);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0.0f, -1.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0.0f, 1.0f, 0.0f);
        }
    }
}
