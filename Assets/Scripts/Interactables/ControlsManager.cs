using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance { get; private set; } // Instance of itself
    public PlayerControls Controls { get; private set; } // The actual controls initialization

    void Awake()
    {
        if (Instance != null && Instance != this) // Prevents duplicates, destroys new instance if already exists
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // persists between scenes

        Controls = new PlayerControls(); // Sets up the only instance of controls
        Controls!.Enable();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("ControlsManager");
            obj.AddComponent<ControlsManager>();
        }
    }

    void OnDestroy() { Controls?.Disable(); } // if Controls exist, disable it
}
