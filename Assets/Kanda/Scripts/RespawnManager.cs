using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector3 spawnRotation = new Vector3(0f, 0f, 0f);

    [Header("Target Objects")] 
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerCamera;

    void Start()
    {
        SetPlayerSpawnPosition();
    }

    private void SetPlayerSpawnPosition()
    {
        if (player == null)
        { 
            MoveController moveController = FindObjectOfType<MoveController>();
            if (moveController != null)
                player = moveController.gameObject;
        }
        
        if (playerCamera == null)
            playerCamera = Camera.main;
    
        if (player != null)
        {
            player.transform.position = spawnPosition;
            player.transform.eulerAngles = spawnRotation;
            Debug.Log($"プレイヤーをリスポーン地点に配置:{spawnPosition}");
        }
        ResetCameraRotation();
    }

    private void ResetCameraRotation()
    {
        if (playerCamera != null)
        {
            CameraController cameraController = playerCamera.GetComponent<CameraController>();
            if (cameraController != null)
            {
                cameraController.ResetCameraRotation();
                Debug.Log("カメラの回転をリセット");
            }
            else
            {
                playerCamera.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void ManualRespawn()
    {
        SetPlayerSpawnPosition();
    }

    public void RespawnAtPosition(Vector3 position, Vector3 rotation)
    {
        spawnPosition = position;
        spawnRotation = rotation;
        SetPlayerSpawnPosition();
    }
}
