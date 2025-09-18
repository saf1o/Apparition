using UnityEngine;

public class StorySceneManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GoToInGameScene();
    }

    public void GoToInGameScene()
    {
        SceneTransitionManager.TransitionToScene("3 InGame");
    }
}
