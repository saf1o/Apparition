using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea;
    [SerializeField] private float searchAngle = 230f;
    [SerializeField] private Vector3 _forward = Vector3.forward;
    
    [Header("Speed Settings")]
    [SerializeField] private float normalSpeed = 0.02f;
    [SerializeField] private float chaseSpeed = 0.04f;
    
    private Animator animator;
    public GameObject player;
    private NavMeshAgent agent;
    public Transform[] points;
    
    public float distance;
    public bool search;
    // 現在の目的地
    public int destPoint;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        
        if (points.Length > 0)
            GotoNextPoint();
    }

    void FixedUpdate()
    {
        // playerが未存在でも動く
        if (player != null) 
            distance = Vector3.Distance(this.transform.position, player.transform.position);
        
        if (distance < 1.0) animator.SetTrigger("Attack");

        if (search)
        {
            // 追跡
            animator.SetBool("Run", true);
            agent.speed = chaseSpeed;
            agent.SetDestination(player.transform.position);
            // transform.position += transform.forward * 0.07f;
            //
            // var dir =  player.transform.position - this. transform.position;
            // // ターゲット方向へ回転
            // var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            // // 回転補正
            // var offsetRotation = Quaternion.LookRotation(_forward, Vector3.forward);
            //
            // transform.rotation = lookAtRotation * offsetRotation * Quaternion.Euler(0, 90, 0);
            // animator.SetBool("Move", false);
            // this.transform.LookAt(player.transform);
        }
        else
        {
            // 巡回
            animator.SetBool("Run", false);
            agent.speed = normalSpeed;
            // transform.position += transform.forward * 0.05f;
            
            // 目的地到着→次の目的地
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
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
            search = false;
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

    void GotoNextPoint()
    {
        // 地点未設定時に返す
        if (points.Length == 0) return;
        // 目標地点へ
        agent.destination = points[destPoint].position;
        // 配列内　目標地点設定　出発地点へ
        destPoint = (destPoint + 1) % points.Length;
        
        // agent.SetDestination(points[destPoint].position);
    }
}
