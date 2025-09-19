using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneManager : MonoBehaviour
{
    void Update()
    {
        // 左クリック　シーン遷移
        if (Input.GetMouseButtonDown(0))
            GoToInGameScene();
    }

    private void GoToInGameScene()
    {
        if (FindObjectOfType<SceneTransitionManager>() != null)
            SceneTransitionManager.TransitionToScene("3 InGame");
        else
        {
            Debug.Log("SceneTransitionManager未検出　直接遷移");
            SceneManager.LoadScene("3 InGame");
        }
        
    }
}
