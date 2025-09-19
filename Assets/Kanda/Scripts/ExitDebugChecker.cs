using UnityEngine;

public class ExitDebugChecker : MonoBehaviour
{
    [Header("Debug Settings")]
    public bool showDebugLogs = true;
    public bool showTriggerArea = true;
    
    private Collider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();

        if (showDebugLogs)
        {
            Debug.Log("===出口デバッグ開始===");
            Debug.Log($"GameObject名: {gameObject.name}");
            Debug.Log($"位置: {transform.position}");

            if (triggerCollider != null)
            {
                Debug.Log($"Collider: {triggerCollider.GetType().Name}");
                Debug.Log($"IsTrigger: {triggerCollider.isTrigger}");
                Debug.Log($"Enabled: {triggerCollider.enabled}");
            }
            else Debug.LogError("Colliderが見つかりません");
            
            ExitTransition exitScript =  GetComponent<ExitTransition>();
            if (exitScript != null)
                Debug.Log("ExitTransitionスクリプト: 見つかりました");
            else
                Debug.Log("ExitTransitionスクリプトが見つかりません");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (showDebugLogs)
        {
            Debug.Log($"OnTriggerEnter:{other.gameObject.name}");
            Debug.Log($"Tag: {other.tag}");
            
            MoveController moveController = other.GetComponent<MoveController>();
            if (moveController != null)
                Debug.Log($"MoveController発見　Key数: {moveController.Key}");

            else
                Debug.Log("MoveControllerが見つかりません");
        }
    }

    void OnTriggerStay(Collider other)
    {
        var controller = other.GetComponent<MoveController>();
        if (showDebugLogs && controller!= null)
        {
            Debug.Log($"プレイヤー滞在中 - Key: {controller.Key}, E押下チャック中");

            if (Input.GetKeyDown(KeyCode.E))
                Debug.Log("Eキーが押されました");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (showDebugLogs)
            Debug.Log($"OnTriggerExit:{other.gameObject.name}");
    }

    void OnDrawGizmosSelected()
    {
        if (showTriggerArea && triggerCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (triggerCollider is BoxCollider box)
                Gizmos.DrawWireCube(box.center, box.size);
            
            else if(triggerCollider is SphereCollider sphere)
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            
            else if(triggerCollider is CapsuleCollider capsule)
                Gizmos.DrawWireSphere(capsule.center, capsule.radius);
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         Debug.Log("F1キーで強制遷移");
    //         SceneTransitionManager.TransitionToScene("4 Outside");
    //     }
    // }
}
