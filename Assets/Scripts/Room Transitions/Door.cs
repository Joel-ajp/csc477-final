using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string targetSceneName;
    public Vector2 spawnPosition;
    
    private static string playerTag = "Player";
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered door trigger. Loading scene: " + targetSceneName);
            
            // Tell the player where to spawn in the next scene
            PlayerMovement.SetNextSpawnPosition(spawnPosition);
            
            // Load the new scene
            SceneManager.LoadSceneAsync(targetSceneName);
        }
    }
}