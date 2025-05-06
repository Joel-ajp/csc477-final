using UnityEngine;

public class SceneEnvironmentActivator : MonoBehaviour
{
    // References to the environments in this scene
    public GameObject environmentA;
    public GameObject environmentB;
    
    private void Start()
    {
        // Wait a frame to ensure the environment manager is initialized
        Invoke("SetupEnvironments", 0.1f);
    }
    
    private void SetupEnvironments()
    {
        // Find the environment manager
        EnvironmentManager manager = EnvironmentManager.Instance;
        
        if (manager != null)
        {
            // Apply the current environment state based on the manager
            int activeIndex = manager.CurrentEnvironmentIndex;
            
            if (environmentA != null)
                environmentA.SetActive(activeIndex == 0);
                
            if (environmentB != null)
                environmentB.SetActive(activeIndex == 1);
                
            Debug.Log($"Scene environment setup: Environment_{(activeIndex == 0 ? 'A' : 'B')} active");
        }
        else
        {
            Debug.LogWarning("EnvironmentManager not found! Using default environment.");
            
            // Default to environment A if no manager exists
            if (environmentA != null)
                environmentA.SetActive(true);
                
            if (environmentB != null)
                environmentB.SetActive(false);
        }
    }
}