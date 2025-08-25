using System;
using UnityEditor;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea;
    [SerializeField] private float searchAngle = 230f;
    
    private Animator animator;
    public GameObject player;
    public float distance;

    public bool search;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (distance < 1.0)
        {
            animator.SetTrigger("Attack");
        }

        if (search == true)
        {
            animator.SetBool("Run", true);
            transform.position += transform.forward * 0.07f;
            this.transform.LookAt(player.transform);
        }
        else
        {
            animator.SetBool("Run", false);
            transform.position += transform.forward * 0.05f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerDirection  = other.transform.position - transform.position;
            var angle = Vector3.Angle(transform.forward, playerDirection);

            if (angle > searchAngle)
            {
                Debug.Log("Player発見");
                
                search = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            search = false;
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position,Vector3.up, 
            Quaternion.Euler(0f,-searchAngle, 0f) * transform.forward, 
            searchAngle * 2f, searchArea.radius);
    }
#endif    
}
