using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;

public class Door : MonoBehaviour
{
    public string targetSceneName;
    public Vector2 spawnPosition;
    
    // Static references to be preserved across scene loads
    private static Vector2 nextSpawnPosition;
    private static string playerTag = "Player";
    private static string cameraTag = "VirtualCamera";
    
    // Persistent GameObject that handles scene transitions
    private static GameObject sceneManager;
    
    void Awake()
    {
        // Ensure we have a persistent object to handle scene transitions
        if (sceneManager == null)
        {
            sceneManager = new GameObject("DoorSceneManager");
            sceneManager.AddComponent<SceneTransitionManager>();
            DontDestroyOnLoad(sceneManager);
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Store the spawn position statically before loading the new scene
            nextSpawnPosition = spawnPosition;
            SceneManager.LoadSceneAsync(targetSceneName);
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Use the persistent object to handle the transition
        if (sceneManager != null)
        {
            var transitionManager = sceneManager.GetComponent<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.HandleSceneLoaded(nextSpawnPosition);
            }
        }
    }
}

// Separate class to handle scene transitions
public class SceneTransitionManager : MonoBehaviour
{
    public void HandleSceneLoaded(Vector2 spawnPosition)
    {
        StartCoroutine(SetupAfterSceneLoad(spawnPosition));
    }
    
    IEnumerator SetupAfterSceneLoad(Vector2 spawnPosition)
    {
        // Wait for end of frame to ensure all objects are initialized
        yield return new WaitForEndOfFrame();
        
        // Wait a bit more to ensure everything is truly loaded
        yield return new WaitForSeconds(0.1f);
        
        // Find the player and move it to the spawn position
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;
            Debug.Log("Player positioned at: " + spawnPosition);
            
            // Find all virtual cameras and update their follow target
            var vcams = GameObject.FindGameObjectsWithTag("VirtualCamera");
            Debug.Log("Found " + vcams.Length + " virtual cameras in the scene");
            
            foreach (var vcamObj in vcams)
            {
                var vcam = vcamObj.GetComponent<CinemachineVirtualCamera>();
                if (vcam != null)
                {
                    vcam.Follow = player.transform;
                    // Force camera update
                    vcam.PreviousStateIsValid = false;
                    vcam.OnTargetObjectWarped(player.transform, spawnPosition - (Vector2)player.transform.position);
                    Debug.Log("Set camera " + vcamObj.name + " to follow player");
                }
            }
        }
        else
        {
            Debug.LogError("Player not found in the scene!");
        }
    }
}