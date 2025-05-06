using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EnvironmentManager : MonoBehaviour
{
    // Singleton instance
    public static EnvironmentManager Instance { get; private set; }
    
    // Environment tracking
    public GameObject[] environments;
    private int currentIndex = 0;
    public int CurrentEnvironmentIndex => currentIndex;
    
    // Standard naming for environments across scenes
    private const string ENV_A_NAME = "Environment_A";
    private const string ENV_B_NAME = "Environment_B";
    [SerializeField] private Image richardGood;
    [SerializeField] private Image richardEvil;
    [SerializeField] private PlayerVariant playerVariant;
    
    // Input controls
    private PlayerControls controls;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Initialize controls
        controls = new PlayerControls();
        
        // Register for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        controls.Player.SwapEnvironment.performed += OnSwapEnvironment;
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SwapEnvironment.performed -= OnSwapEnvironment;
        controls.Player.Disable();
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetupEnvironmentsAfterSceneLoad());
    }
    
    private IEnumerator SetupEnvironmentsAfterSceneLoad()
    {
        // Wait to ensure all objects are initialized
        yield return new WaitForEndOfFrame();
        
        // Find environments in the new scene
        FindEnvironmentsInScene();

        FindRichardUI();



        
        // Apply the current environment state
        ApplyCurrentEnvironmentState();
        
        Debug.Log($"Applied environment state in new scene: Environment_{(currentIndex == 0 ? 'A' : 'B')} active");
    }
    
    private void FindEnvironmentsInScene()
    {
        // Find Environment_A and Environment_B in the current scene
        GameObject envA = GameObject.Find(ENV_A_NAME);
        GameObject envB = GameObject.Find(ENV_B_NAME);
        
        // Store references in our array
        environments = new GameObject[2];
        environments[0] = envA;
        environments[1] = envB;
        
        // Log what we found
        Debug.Log($"Found environments in scene: A={envA != null}, B={envB != null}");
    }
    
    private void ApplyCurrentEnvironmentState()
    {
        // Safety check
        if (environments == null || environments.Length < 2)
        {
            Debug.LogWarning("Cannot apply environment state - environments not found");
            return;
        }
        
        // Activate the current environment, deactivate the other
        for (int i = 0; i < environments.Length; i++)
        {
            if (environments[i] != null)
            {
                environments[i].SetActive(i == currentIndex);
            }
        }
        UpdateRichardState();
    }

    private void OnSwapEnvironment(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        
        Debug.Log("Swap Environment Pressed");
        
        // Safety check
        if (environments == null || environments.Length < 2)
        {
            Debug.LogWarning("Cannot swap environments - environments not found");
            return;
        }
        
        // Deactivate current environment
        if (environments[currentIndex] != null)
            environments[currentIndex].SetActive(false);
            print("Deactivated Environment_" + (currentIndex == 0 ? 'A' : 'B'));
            
        // Switch to next environment
        currentIndex = (currentIndex + 1) % environments.Length;
        
        // Activate new environment
        if (environments[currentIndex] != null)
            environments[currentIndex].SetActive(true);
            
        Debug.Log($"Switched to Environment_{(currentIndex == 0 ? 'A' : 'B')}");
        UpdateRichardState();
      
    }

   private void UpdateRichardState(){
    bool inA = (currentIndex == 0);
    if (richardGood  != null) richardGood.gameObject.SetActive(inA);
    if (richardEvil != null) richardEvil.gameObject.SetActive(!inA);
     playerVariant?.SetVariant(inA);
    }

    private void FindRichardUI(){
    var goGood = GameObject.Find("richard");
    var goEvil = GameObject.Find("richard (evil)");

    if (goGood != null)  richardGood  = goGood.GetComponent<Image>();
    if (goEvil != null)  richardEvil  = goEvil.GetComponent<Image>();
    }

}