using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button startButton;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(GoToStoryScene);
    }

    private void GoToStoryScene()
    {
        SceneManager.LoadScene("2 Story");
    }
}
