using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    void Awake()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>(true);
        
        Debug.Log($"EventSystem数: {eventSystems.Length}");
        
        bool hasDontDestroyEventSystem = false;
        EventSystem dontDestroyEventSystem = null;
        foreach (EventSystem es in eventSystems)
        {
            if (es.gameObject.scene.name == "DontDestroyOnLoad")
            {
                hasDontDestroyEventSystem = true;
                dontDestroyEventSystem = es;
                Debug.Log("DontDestroyOnLoadのEventSystem検知");
                //Destroy(es.gameObject);//
                break;
            }
        }

        if (hasDontDestroyEventSystem)
        {
            EventSystem currentEventSystem = GetComponent<EventSystem>();
            if (currentEventSystem != null && currentEventSystem != dontDestroyEventSystem)
            {
                Debug.Log("現在のEventSystem削除");
                Destroy(gameObject);
                return;
            }
        }
        Debug.Log("シーンのEventSystem使用");
    }

    void Start()
    {
        if (transform.parent == null && gameObject.scene.name == "DontDestroyOnLoad")
        {
            Debug.Log($"EventSystem `{gameObject.name}` は通常シーンに留まります");
        }
    }
}
