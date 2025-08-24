using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Animator animator;

    public float stamina;

    public Slider stamina_slider;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Walk", true);
            // transform.position += transform.forward * (speed * Time.deltaTime);
            
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position += transform.forward * 0.02f;
                stamina = stamina - (Time.deltaTime * 10);
            }
            else
            {
                transform.position += transform.forward * 0.01f;
            }
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

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            stamina = stamina +  (Time.deltaTime * 2);
        }

        const float MIN = 0;
        const float MAX = 100;
        stamina = Mathf.Clamp(stamina, MIN, MAX);
        
        stamina_slider.value = stamina;
    }
}
