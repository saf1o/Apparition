using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTransition : MonoBehaviour
{
    [Header("Scene Transition")] [SerializeField]
    private string nextSceneName = "GameResult";

    [Header("Clear Conditions")] 
    [SerializeField] private int requiredKayCount = 1;
    [SerializeField] private bool showClearMessage = true;

    [Header("UI Elements")] 
    [SerializeField] private GameObject clearMassageUI;
    [SerializeField] private GameObject exitPromptUI;
    
    private bool playerInRange = false;
    private MoveController playerController;

    void Start()
    {
        if (clearMassageUI != null)
            clearMassageUI.SetActive(false);
        if (exitPromptUI != null)
            exitPromptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && playerController == null)
        {
            if (IsGameCleared())
            {
                if (exitPromptUI != null)
                    exitPromptUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    TransitionToNextScene();
            }

            else
            {
                if (clearMassageUI != null) clearMassageUI.SetActive(true);
            }
        }
        else
        {
            if (clearMassageUI != null) clearMassageUI.SetActive(false);
            if (exitPromptUI != null) exitPromptUI.SetActive(false);
        }
    }

    private bool IsGameCleared()
    {
        if (playerController == null) return false;
        
        return playerController.Kay >= requiredKayCount;
    }

    private void TransitionToNextScene()
    {
        Debug.Log($"シーン遷移: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
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
