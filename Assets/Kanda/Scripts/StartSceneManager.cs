using UnityEngine;
using UnityEngine.UI;

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
        SceneTransitionManager.TransitionToScene("2 Story");
    }
}
