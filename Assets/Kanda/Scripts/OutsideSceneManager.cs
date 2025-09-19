using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutsideSceneManager : MonoBehaviour
{
    [Header("Bus Stop Settings")] 
    [SerializeField] private Transform busStopPosition;
    [SerializeField] private float busStopRange = 2f;

    [Header("Scene Transition")] 
    [SerializeField] private string nextSceneName = "5 BusClear";
    [SerializeField] private float fadeToBlackDuration = 2f;
    [SerializeField] private float waitAfterFadeTime = 1f;
    
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private GameObject busApproachingUI;
    
    private GameObject player;
    private bool playerAtBusStop = false;
    private bool isTransitioning = false;

    void Start()
    {
        // プレイヤー検知
        MoveController moveController = FindObjectOfType<MoveController>();
        if (moveController != null)
            player = moveController.gameObject;
        // UI初期化
        if (busApproachingUI != null)
            busApproachingUI.SetActive(false);
        // パネル初期化
        if (fadePanel != null)
        {
            fadePanel.alpha = 0;
            fadePanel.gameObject.SetActive(true);
        }
        else
        {
            CreateFadePanel();
        }
        // player = FindObjectOfType<MoveController>()?.gameObject;
    }

    void Update()
    {
        if (!isTransitioning)
        {
            CheckPlayerAtBusStop();
        }
    }

    private void CheckPlayerAtBusStop()
    {
        if (player != null && busStopPosition != null)
        {
            float distance = Vector3.Distance(player.transform.position, busStopPosition.position);
            if (distance <= busStopRange && !playerAtBusStop)
            {
                playerAtBusStop = true;
                Debug.Log("バス停検知　フェード開始");
                StartBusSequence();
            }
            else if (distance > busStopRange * 1.2f && playerAtBusStop && isTransitioning)
            {
                playerAtBusStop = false;
                if (busApproachingUI != null)
                    busApproachingUI.SetActive(false);
            }
        }
    }

    private void StartBusSequence()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        
        if (busApproachingUI != null)
            busApproachingUI.SetActive(true);
        
        Debug.Log("フェード開始");
        StartCoroutine(BusTransitionSequence());
    }

    private System.Collections.IEnumerator BusTransitionSequence()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("フェードアウト開始");
        yield return StartCoroutine(FadeToBlack());
        
        yield return new WaitForSeconds(waitAfterFadeTime);
        
        Debug.Log($"シーン遷移: {nextSceneName}");

        if (FindObjectOfType<SceneTransitionManager>() != null)
        {
            SceneTransitionManager.TransitionToScene(nextSceneName);
        }
        else SceneManager.LoadScene(nextSceneName);
    }

    private System.Collections.IEnumerator FadeToBlack()
    {
        if (fadePanel == null) yield break;
        float elapsed = 0f;
        while (elapsed < fadeToBlackDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeToBlackDuration);
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 1f;
        Debug.Log("フェードアウト完了");
    }

    private void CreateFadePanel()
    {
        // Canvas
        GameObject canvasGO = new GameObject("FadeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // フェードパネル
        GameObject panelGO = new GameObject("FadePanel");
        panelGO.transform.SetParent(canvas.transform, false);
        
        UnityEngine.UI.Image image = panelGO.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;
        
        // フルスクリーン
        RectTransform rect = panelGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        fadePanel = panelGO.AddComponent<CanvasGroup>();
        fadePanel.alpha = 0f;
        
        Debug.Log("フェードパネルを自動作成しました");
    }

    void OnDrawGizmosSelected()
    {
        if (busStopPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(busStopPosition.position, busStopRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(busStopPosition.position, busStopRange * 1.2f);
        }
    }
}
