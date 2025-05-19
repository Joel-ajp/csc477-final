using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the cleanup of DontDestroyOnLoad objects when returning to the Main Menu scene.
/// Attach this script to a GameObject in your Main Menu scene.
/// </summary>
public class SceneCleaner : MonoBehaviour
{
    [Tooltip("Tags of objects that should be destroyed when returning to Main Menu")]
    [SerializeField] private string[] tagsToDestroy = { "DontDestroyObject" };
    
    [Tooltip("Names of objects that should be destroyed when returning to Main Menu")]
    [SerializeField] private string[] namesToDestroy = { 
        "ControlsManager1",
        "[Debug Updater]",
        "ShopStateManager",
        "GameManager",
        "hud (use this one)",
    };

    private void Start()
    {
        // Clean up DontDestroyOnLoad objects when Main Menu loads
        CleanupDontDestroyObjects();
    }

    /// <summary>
    /// Finds and destroys all objects with the specified tags or names
    /// that were marked with DontDestroyOnLoad
    /// </summary>
    private void CleanupDontDestroyObjects()
    {
        // First, try to find objects by tag
        foreach (string tag in tagsToDestroy)
        {
            if (string.IsNullOrEmpty(tag)) continue;
            
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objectsToDestroy)
            {
                Debug.Log($"Destroying DontDestroyOnLoad object with tag: {tag}, name: {obj.name}");
                Destroy(obj);
            }
        }
        
        // Then try to find objects by name
        foreach (string objName in namesToDestroy)
        {
            if (string.IsNullOrEmpty(objName)) continue;
            
            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                Debug.Log($"Destroying DontDestroyOnLoad object with name: {objName}");
                Destroy(obj);
            }
        }
    }

    /// <summary>
    /// Public method to add more objects to destroy by tag
    /// </summary>
    public void AddTagToDestroy(string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            // Create a new array with space for the new tag
            string[] newTags = new string[tagsToDestroy.Length + 1];
            tagsToDestroy.CopyTo(newTags, 0);
            newTags[tagsToDestroy.Length] = tag;
            tagsToDestroy = newTags;
        }
    }
}