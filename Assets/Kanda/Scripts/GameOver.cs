using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E))
            SceneManager.LoadScene("GameStart");
    }
}
