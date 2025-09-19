using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTransition : MonoBehaviour
{
    [Header("Scene Transition")] 
    [SerializeField] private string nextSceneName = "4 OutSide";

    [Header("Clear Conditions")] 
    [SerializeField] private int requiredKayCount = 1;
    [SerializeField] private bool showClearMessage = true;

    [Header("UI Elements")] 
    [SerializeField] private GameObject clearMassageUI;
    [SerializeField] private GameObject exitPromptUI;
    [SerializeField] private GameObject needKeyMessageUI;
    
    private bool playerInRange = false;
    private MoveController playerController;

    void Start()
    {
        if (clearMassageUI != null)
            clearMassageUI.SetActive(false);
        if (exitPromptUI != null)
            exitPromptUI.SetActive(false);
        if (needKeyMessageUI != null)
            needKeyMessageUI.SetActive(false);
    }

    void FixedUpdate()
    {
        if (playerInRange && playerController == null)
        {
            Debug.Log($"プレイヤー滞在中 - Kay: {playerController.Key}/{requiredKayCount}");

            bool canExit = IsGameCleared();
            Debug.Log($"出口使用可: {canExit}");
            // クリア条件
            if (canExit)
            {
                Debug.Log("鍵あり - Exit Prompt表示");
                if (exitPromptUI != null)
                    exitPromptUI.SetActive(true);
                if (clearMassageUI != null)
                    clearMassageUI.SetActive(false);
                if (needKeyMessageUI != null)
                    needKeyMessageUI.SetActive(false);
                
                // Eキーでシーン遷移
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E押下　シーン遷移開始");
                    TransitionToNextScene();
                }
            }

            else
            {
                // 鍵がない場合　出口使用不可
                Debug.Log("鍵なし - Need Kay Message表示");
                if (clearMassageUI != null) 
                    clearMassageUI.SetActive(true);
                else
                    Debug.Log("needKayMessageUI null");
                if (exitPromptUI != null)
                    exitPromptUI.SetActive(false);
                if (needKeyMessageUI != null)
                    needKeyMessageUI.SetActive(false);
                
                Debug.Log($"鍵が必要です　現在: {playerController.Key}/{requiredKayCount}");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("鍵がないため出口使用不可");
                    ShowNeedKeyFeedback();
                }
            }
        }
        else
        {
            // プレイヤーが範囲外　UI非表示
            if (clearMassageUI != null) 
                clearMassageUI.SetActive(false);
            
            if (exitPromptUI != null) 
                exitPromptUI.SetActive(false);
            
            if (needKeyMessageUI != null)
                needKeyMessageUI.SetActive(false);
        }
    }

    private bool IsGameCleared()
    {
        if (playerController == null) return false;
        bool hasEnoughKeys = playerController.Key >= requiredKayCount;
        Debug.Log($"鍵チェック: {playerController.Key}/{requiredKayCount} = {hasEnoughKeys}");
        return hasEnoughKeys;
    }

    private void TransitionToNextScene()
    {
        if (!IsGameCleared())
        {
            Debug.Log("クリア条件未達成　遷移不可");
            ShowNeedKeyFeedback();
            return;
        }
        Debug.Log($"シーン遷移: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }

    private void ShowNeedKeyFeedback()
    {
        Debug.Log("鍵を見つけてから出口を使用してください");
        StartCoroutine(ShakeUI());
    }

    private System.Collections.IEnumerator ShakeUI()
    {
        if (needKeyMessageUI != null)
        {
            Vector3 originalPos = needKeyMessageUI.transform.localPosition;
            float shakeTime = 0.3f;
            float shakePower = 5f;

            while (shakeTime > 0)
            {
                needKeyMessageUI.transform.localPosition
                    = originalPos + new Vector3(Random.Range(-shakePower, shakePower)
                        , Random.Range(-shakePower, shakePower), 0);
                shakeTime -= Time.deltaTime;
                yield return null;
            }
            needKeyMessageUI.transform.localPosition = originalPos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        MoveController controller = other.GetComponent<MoveController>();
        if (controller != null)
        {
            playerInRange = true;
            playerController = controller;
            Debug.Log("プレーヤーが出口エリアに入りました");
        }
    }

    void OnTriggerExit(Collider other)
    {
        MoveController controller = other.GetComponent<MoveController>();
        if (controller != null)
        {
            playerInRange = false;
            playerController = null;
            Debug.Log("プレーヤーが出口エリアから出ました");
        }
    }
}
