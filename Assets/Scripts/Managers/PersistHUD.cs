using UnityEngine;

public class PersistHUD : MonoBehaviour
{
    private static PersistHUD _instance;

    void Awake()
    {
       
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
