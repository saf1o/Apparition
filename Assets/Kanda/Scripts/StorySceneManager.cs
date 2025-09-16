using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GoToInGameScene();
    }

    public void GoToInGameScene()
    {
        SceneManager.LoadScene("3 InGame");
    }
}
