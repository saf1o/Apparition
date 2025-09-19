using UnityEngine;

public class SimpleRespawnManager : MonoBehaviour
{
    [Header("Respawn Position")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector3 spawnRotation = new Vector3(0f, 0f, 0f);
    
    [Header("Debug")]
    [SerializeField] private bool showSpawnGizmo = true;
    
    void Start()
    {
        Debug.Log("SimpleRespawnManager開始");
        Debug.Log($"設定されたリスポーン位置: {spawnPosition}");
        
        // 複数のタイミングでリスポーン処理を試行
        Invoke("DoRespawn", 0.1f);
        Invoke("DoRespawn", 0.3f);
        Invoke("DoRespawn", 0.5f);
    }
    
    private void DoRespawn()
    {
        Debug.Log($"リスポーン処理実行 (時間: {Time.time})");
        
        // DontDestroyOnLoadのプレイヤーを探す
        MoveController[] allControllers = FindObjectsOfType<MoveController>();
        MoveController targetController = null;
        
        Debug.Log($"MoveController数: {allControllers.Length}");
        
        foreach (MoveController controller in allControllers)
        {
            Debug.Log($"MoveController発見: {controller.gameObject.name} at {controller.transform.position}");
            
            // DontDestroyOnLoadオブジェクトを優先
            if (controller.gameObject.scene.name == "DontDestroyOnLoad")
            {
                targetController = controller;
                Debug.Log("DontDestroyOnLoadのプレイヤーを選択");
                break;
            }
            else if (targetController == null)
            {
                targetController = controller;
            }
        }
        
        if (targetController != null)
        {
            GameObject player = targetController.gameObject;
            
            Debug.Log($"対象プレイヤー: {player.name}");
            Debug.Log($"移動前位置: {player.transform.position}");
            Debug.Log($"移動前回転: {player.transform.eulerAngles}");
            
            // 強制的に位置をリセット
            player.transform.position = spawnPosition;
            player.transform.eulerAngles = spawnRotation;
            
            Debug.Log($"移動後位置: {player.transform.position}");
            Debug.Log($"移動後回転: {player.transform.eulerAngles}");
            
            // Rigidbodyがあればリセット
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                Debug.Log("Rigidbody速度リセット");
            }
            
            // CharacterControllerがあればリセット
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = spawnPosition;
                cc.enabled = true;
                Debug.Log("CharacterController経由でリセット");
            }
            
            // カメラリセット
            ResetPlayerCamera(player);
            
            Debug.Log("✅ リスポーン完了");
            
            // 結果確認
            StartCoroutine(VerifyRespawn(player));
        }
        else
        {
            Debug.LogError("❌ MoveControllerが見つかりません！");
        }
    }
    
    private void ResetPlayerCamera(GameObject player)
    {
        Camera playerCamera = player.GetComponentInChildren<Camera>();
        if (playerCamera != null)
        {
            Debug.Log("カメラ発見、リセット実行");
            
            CameraController camController = playerCamera.GetComponent<CameraController>();
            if (camController != null)
            {
                camController.ResetCameraRotation();
                Debug.Log("CameraController経由でリセット");
            }
            else
            {
                // 直接リセット
                playerCamera.transform.localRotation = Quaternion.identity;
                Debug.Log("直接カメラ回転リセット");
            }
        }
        else
        {
            Debug.LogWarning("プレイヤーカメラが見つかりません");
        }
    }
    
    private System.Collections.IEnumerator VerifyRespawn(GameObject player)
    {
        yield return new WaitForSeconds(0.1f);
        
        Debug.Log("=== リスポーン結果確認 ===");
        Debug.Log($"最終位置: {player.transform.position}");
        Debug.Log($"目標位置: {spawnPosition}");
        Debug.Log($"距離差: {Vector3.Distance(player.transform.position, spawnPosition)}");
        
        if (Vector3.Distance(player.transform.position, spawnPosition) > 0.5f)
        {
            Debug.LogWarning("⚠️ リスポーン位置が目標と異なります！再試行...");
            player.transform.position = spawnPosition;
        }
    }
    
    // 手動リスポーン用
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Rキーで手動リスポーン");
            DoRespawn();
        }
    }
    
    // Gizmo表示
    void OnDrawGizmos()
    {
        if (showSpawnGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnPosition, Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(spawnPosition, Vector3.up * 2f);
        }
    }
}
