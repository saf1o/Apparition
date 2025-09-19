using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleExitTest : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string nextSceneName = "4 Outside";
    [SerializeField] private int requiredKeys = 1;
    
    [Header("UI Messages")]
    [SerializeField] private GameObject needKeyMessage;
    [SerializeField] private GameObject canExitMessage;
    
    private bool playerInRange = false;
    private MoveController playerController;
    
    void Start()
    {
        Debug.Log("SimpleExitTest開始");
        
        // UI初期化
        if (needKeyMessage != null) 
            needKeyMessage.SetActive(false);
        
        if (canExitMessage != null) 
            canExitMessage.SetActive(false);
    }
    
    void Update()
    {
        // デバッグ用 - Tキーで強制遷移
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Tキーで強制シーン遷移");
            SceneManager.LoadScene(nextSceneName);
            return;
        }
        
        // 距離ベース判定を併用（より安定）
        MoveController player = FindObjectOfType<MoveController>();
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            // 2メートル以内
            bool isNearExit = distance <= 2f; 
            
            // トリガーと距離の両方をチェック
            bool shouldShowUI = playerInRange || isNearExit;
            
            if (shouldShowUI)
            {
                if (playerController == null) 
                    playerController = player;
                
                int currentKeys = playerController.Key;
                bool hasEnoughKeys = currentKeys >= requiredKeys;
                
                // ログの頻度を下げる（1秒に1回程度）
                if (Time.time % 1f < 0.1f)
                {
                    Debug.Log($"出口付近 - Keys: {currentKeys}/{requiredKeys}, " +
                              $"距離: {distance:F1}m, 遷移可能: {hasEnoughKeys}");
                }
                
                // UI更新
                if (hasEnoughKeys)
                {
                    ShowCanExitMessage();
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("✅ Eキー押下 - 条件満たしているので遷移します");
                        SceneManager.LoadScene(nextSceneName);
                    }
                }
                else
                {
                    ShowNeedKeyMessage();
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.LogWarning($"❌ Eキー押下 - 鍵が足りません！ {currentKeys}/{requiredKeys}");
                    }
                }
            }
            else
            {
                HideAllMessages();
                
                // 距離が遠い場合のみクリア
                if (playerController != null && !playerInRange)
                    playerController = null;
            }
        }
        else HideAllMessages();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoveController>() != null)
        {
            playerInRange = true;
            playerController = other.GetComponent<MoveController>();
            Debug.Log($"✅ 【ENTER】プレイヤー検知 - Key数: {playerController.Key}");
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        // OnTriggerStayで継続的に確認
        if (other.GetComponent<MoveController>() != null && playerController == null)
        {
            playerInRange = true;
            playerController = other.GetComponent<MoveController>();
            Debug.Log($"✅ 【STAY】プレイヤー再検知 - Key数: {playerController.Key}");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MoveController>() != null)
        {
            // 少し遅延させてからクリア（チラつき防止）
            Invoke("DelayedClearPlayer", 0.2f);
            Debug.Log("【EXIT】プレイヤーが出口から離れました（0.2秒後にクリア）");
        }
    }
    
    private void DelayedClearPlayer()
    {
        // 再度距離チェックしてから本当にクリア
        if (playerController != null)
        {
            float distance = Vector3.Distance(transform.position, playerController.transform.position);
            // 3メートル以上離れた場合のみクリア
            if (distance > 3f) 
            {
                playerInRange = false;
                playerController = null;
                Debug.Log("プレイヤー情報をクリアしました");
            }
            else Debug.Log("まだ近くにいるのでクリアを中止");
        }
    }
    
    private void ShowNeedKeyMessage()
    {
        if (needKeyMessage != null) needKeyMessage.SetActive(true);
        if (canExitMessage != null) canExitMessage.SetActive(false);
    }
    
    private void ShowCanExitMessage()
    {
        if (needKeyMessage != null) needKeyMessage.SetActive(false);
        if (canExitMessage != null) canExitMessage.SetActive(true);
    }
    
    private void HideAllMessages()
    {
        if (needKeyMessage != null) needKeyMessage.SetActive(false);
        if (canExitMessage != null) canExitMessage.SetActive(false);
    }
    
    // Gizmos表示
    void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            
            if (col is BoxCollider box)
                Gizmos.DrawWireCube(box.center, box.size);
        }
    }
}