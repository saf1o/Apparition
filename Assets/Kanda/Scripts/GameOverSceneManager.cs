using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSceneManager : MonoBehaviour
{
    [SerializeField] private Button retryButton;

    void Start()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RetryGame();
    }

    private void RetryGame()
    {
        SceneManager.LoadScene("2 Story");
    }
}
