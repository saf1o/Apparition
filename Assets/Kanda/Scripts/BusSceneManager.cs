using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BusSceneManager : MonoBehaviour
{
    [Header("Bus Sequence Settings")] 
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float busWaitTime = 2f;
    [SerializeField] private float busRideDuration = 5f;
    [SerializeField] private float clearDisplayTime = 3f;
    [SerializeField] private float autoReturnTime = 5f;
    
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private GameObject busRideUI;
    [SerializeField] private GameObject clearUI;
    [SerializeField] private Button restartButton;
    
    private bool canReturn = false;

    void Start()
    {
        // 初期値
        if (busRideUI != null) busRideUI.SetActive(true);
        if (clearUI != null) clearUI.SetActive(true);
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(ReturnToStart);
            restartButton.gameObject.SetActive(false);
        }

        StartCoroutine(BusAndClearSequence());
    }

    void Update()
    {
        if (canReturn && Input.GetKeyDown(KeyCode.K))
        {
            ReturnToStart();
        }
    }

    private IEnumerator BusAndClearSequence()
    {
        // 暗くなる
        yield return StartCoroutine(FadeToBlack());
        // 待機
        // yield return new WaitForSeconds(busWaitTime);
        // Debug.Log("バス乗車");
        // yield return new WaitForSeconds(busRideDuration);
        // // バス演出UI非表示
        // if (busRideUI != null) busRideUI.SetActive(false);
        // // 明るくなる
        // yield return StartCoroutine(FadeToBlack());
        // // クリア画面表示
        // if (clearUI != null) clearUI.SetActive(true);
        // if (restartButton != null) restartButton.gameObject.SetActive(true);
        // Debug.Log("ゲームクリア");
        // // 待機後操作可能にする
        // yield return new WaitForSeconds(clearDisplayTime);
        // canReturn = true;
        // 自動でスタート画面へ
        yield return new WaitForSeconds(autoReturnTime);
        ReturnToStart();
    }

    private IEnumerator FadeToBlack()
    {
        if (fadePanel != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadePanel.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                yield return null;
            }
            fadePanel.alpha = 1f;
        }
        else yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeFromBlack()
    {
        if (fadePanel != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration * 0.5f)
            {
                elapsed += Time.deltaTime;
                fadePanel.alpha = Mathf.Lerp(1f, 0f, elapsed / (fadeDuration * 0.5f));
                yield return null;
            }
            fadePanel.alpha = 0f;
        }
    }

    private void ReturnToStart()
    {
        Debug.Log("スタート画面へ");
        
        if (FindObjectOfType<SceneTransitionManager>() != null)
            SceneTransitionManager.TransitionToScene("1 Start");
        else
            SceneManager.LoadScene("1 Start");
    }
}
