using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SceneManager.LoadScene("InGame");
    }
}
