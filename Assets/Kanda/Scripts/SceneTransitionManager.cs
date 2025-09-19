using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")] 
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private Color fadeColor = Color.black;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private Image fadeImage;
    
    private static SceneTransitionManager instance;
    private bool isTransitioning = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (fadePanel == null) 
                CreateFadePanel();
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        if (fadeImage != null) 
            StartCoroutine(FadeIn());
    }

    // フェードpanel自動生成
    private void CreateFadePanel()
    {
        GameObject canvas = new GameObject("TransitionCanvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasComponent.sortingOrder = 999;
        
        CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvas.AddComponent<GraphicRaycaster>();
        DontDestroyOnLoad(canvas);
        
        fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(canvas.transform, false);
        
        fadeImage = fadePanel.AddComponent<Image>();
        fadeImage.color = fadeColor;
        
        RectTransform rect = fadePanel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        fadePanel.SetActive(true);
    }

    public static void TransitionToScene(string sceneName)
    {
        if (instance != null && !instance.isTransitioning)
        {
            instance.StartCoroutine(instance.TransitionCoroutine(sceneName));
        }
        // else
        // {
        //     UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        // }
    }

    public static void TransitionToScene(int sceneIndex)
    {
        if (instance != null && !instance.isTransitioning)
        {
            instance.StartCoroutine(instance.TransitionCoroutine(sceneIndex));
        }
    }

    private IEnumerator TransitionCoroutine(string sceneName)
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeOut());
        
        SceneManager.LoadScene(sceneName);
        
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(FadeIn());
        
        isTransitioning = false;
    }

    private IEnumerator TransitionCoroutine(int sceneIndex)
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeOut());
        
        SceneManager.LoadScene(sceneIndex);
        
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(FadeIn());
        
        isTransitioning = false;
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;

        float elapsed = 0f;
        Color startColor = fadeColor;
        startColor.a = 0f;
        Color endColor = fadeColor;
        endColor.a = 1f;
        
        fadePanel.SetActive(true);

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / transitionDuration);
            
            Color currentColor = fadeColor;
            currentColor.a = alpha;
            fadeImage.color = currentColor;
            
            yield return null;
        }
        Color finalColor = fadeColor;
        finalColor.a = 1f;
        fadeImage.color = finalColor;
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        float  elapsed = 0f;
        Color startColor = fadeColor;
        startColor.a = 1f;
        Color endColor = fadeColor;
        endColor.a = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / transitionDuration);
            
            Color currentColor = fadeColor;
            currentColor.a = alpha;
            fadeImage.color = currentColor;
            
            yield return null;
        }
        Color finalColor = fadeColor;
        finalColor.a = 0f;
        fadeImage.color = finalColor;
        
        fadePanel.SetActive(false);
    }
}
