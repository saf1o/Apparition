using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Settings")] 
    [SerializeField] private bool hideCursorInGame = true;
    [SerializeField] private bool lockCursorInGame = true;

    [Header("Scene Settings")] 
    [SerializeField] private string[] gameScenes = { "3 InGame", "4 Outside" };
    [SerializeField] private string[] menuScenes = { "1 Start", "2 Story", "5 BusClear"};

    private static CursorManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else 
            Destroy(gameObject);
    }

    void Start()
    {
        SetCursorForCurrentScene();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCursor();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"シーン読み込み: {scene.name}");
        Invoke("SetCursorForCurrentScene", 0.1f);
    }

    private void SetCursorForCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"カーソル: {currentSceneName}");
        
        if (IsGameScene(currentSceneName))
            SetCursorState(false, true);
        
        else if (IsMenuScene(currentSceneName))
            SetCursorState(true, false);
        
        else
            SetCursorState(true, false);
    }

    private bool IsGameScene(string sceneName)
    {
        foreach (string gameScene in gameScenes)
        {
            if (sceneName.Contains(gameScene) || sceneName ==  gameScene)
                return true;
        }
        return false;
    }
    
    private bool IsMenuScene(string sceneName)
    {
        foreach (string menuScene in gameScenes)
        {
            if (sceneName.Contains(menuScene) || sceneName ==  menuScene)
                return true;
        }
        return false;
    }

    private void SetCursorState(bool visible, bool locked)
    {
        if (visible)
        {
            Cursor.visible = true;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        }
        else
        {
            if (hideCursorInGame)
                Cursor.visible = false;
            if (lockCursorInGame)
                Cursor.lockState = CursorLockMode.Locked;
        }
        Debug.Log($"カーソル設定: Visible={Cursor.visible}, LockState={Cursor.lockState}");
    }

    public static void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("カーソル強制表示");
    }

    public static void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("カーソル強制非表示");
    }

    public static void ToggleCursor()
    {
        if (Cursor.visible)
            HideCursor();
        else
            ShowCursor();
    }

    public void SetHideCursorInGame(bool hide)
    {
        hideCursorInGame = hide;
        SetCursorForCurrentScene();
    }

    public void SetLockCursorInGame(bool lockCursor)
    {
        lockCursorInGame = lockCursor;
        SetCursorForCurrentScene();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
            Invoke("SetLockCursorForCurrentScene", 0.1f);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}