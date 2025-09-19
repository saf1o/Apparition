using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    private static GameObject playerInstance;

    [Header("Player Persistence")] [SerializeField]
    private bool keepPlayerBetweenScenes = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (keepPlayerBetweenScenes)
                MakePlayerPersistent();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"シーン読み込み完了: {scene.name}");
        StartCoroutine(DelayedPlayerSetup());
    }

    private System.Collections.IEnumerator DelayedPlayerSetup()
    {
        yield return new WaitForSeconds(0.1f);
        RespawnManager respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager != null)
        {
            Debug.Log("RespawnManager検知 - リスポーン処理実行");
            respawnManager.ManualRespawn();
        }
        else
        {
            Debug.Log("RespawnManager未検知");
            CheckPLayerStatus();
        }
    }

    private void MakePlayerPersistent()
    {
        MoveController moveController = FindObjectOfType<MoveController>();
        if (moveController != null)
        {
            playerInstance = moveController.gameObject;
            DontDestroyOnLoad(playerInstance);
            Debug.Log("プレイヤー永続化");
        }
    }

    private void CheckPLayerStatus()
    {
        if (playerInstance == null)
        {
            Debug.Log($"プレイヤー位置: {playerInstance.transform.position}");
            Camera playerCamera = playerInstance.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                Debug.Log("カメラ検知");
                CameraController cameraController = playerCamera.GetComponent<CameraController>();
                if (cameraController != null)
                {
                    Debug.Log("CameraController検知");
                    cameraController.ResetCameraRotation();
                }
                else
                    Debug.LogWarning("CameraController未検知");
            }
            else
                Debug.LogWarning("Player Noting Camera");
        }
        else
            Debug.LogWarning("PlayerInstance Null");
    }

    public static GameObject GetPlayer()
    {
        return playerInstance;
    }

    public static void SetPlayerPosition(Vector3 position, Vector3 rotation)
    {
        if (playerInstance != null)
        {
            playerInstance.transform.position = position;
            playerInstance.transform.eulerAngles = rotation;

            Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            Debug.Log($"プレイヤー位置　リセット: {position}");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}