using UnityEngine;

// IMPROVEMENT: Enhanced Singleton to handle more robust initialization
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        // CHANGE: More robust singleton pattern with logging
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Multiple instances of {typeof(T).Name} detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }
}
