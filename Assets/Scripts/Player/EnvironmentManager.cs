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
    [SerializeField] private PlayerMovement playerMovement;

    // Animation parameters
    private float transformationDuration = 1.5f; // Adjust based on animation length
    private float flashDelay = 1.75f; // When to trigger flash during animation (in seconds)
    private bool isTransforming = false;
    private const string TRANSFORMATION_ANIM = "transformation";

    // Screen flash effect
    private Image screenFlash;
    private float flashDuration = 0.5f;
    private Color flashColor = Color.red;

    // Input controls
    private PlayerControls controls;
    // Prevent dimension swapping while in menus/dialogue
    public bool swapAllowed;

    // Reference to player's Animator component
    private Animator playerAnimator;

    private IEnumerator PlayScreenFlash()
    {
        // Ensure we have a screen flash image

        if (screenFlash == null)
        {
            CreateScreenFlash();
        }

        // Flash in
        float elapsedTime = 0;
        Color startColor = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / (flashDuration / 2));
            screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // Ensure we hit peak opacity
        screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1);

        // Flash out
        elapsedTime = 0;
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / (flashDuration / 2));
            screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // Ensure we end fully transparent
        screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }

    private void OnValidate()
    {
        // Keep timing values in valid ranges
        transformationDuration = Mathf.Max(0.1f, transformationDuration);
        flashDelay = Mathf.Clamp(flashDelay, 0, transformationDuration);
        flashDuration = Mathf.Max(0.1f, flashDuration);
    }

    private void Awake()
    {
        //swap now allowed when game starts
        //enabled when arrive in start room scene
        swapAllowed = true;

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

        // Create screen flash if not assigned
        if (screenFlash == null)
        {
            CreateScreenFlash();
        }

        // Register for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void CreateScreenFlash()
    {
        // Create a canvas for the flash if needed
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("FlashCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // Ensure it's on top
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            DontDestroyOnLoad(canvasObj);
        }

        // Create flash image
        GameObject flashObj = new GameObject("ScreenFlash");
        flashObj.transform.SetParent(canvas.transform, false);

        // Set up RectTransform to cover screen
        RectTransform rectTransform = flashObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        // Add and configure Image component
        screenFlash = flashObj.AddComponent<Image>();
        screenFlash.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0); // Start transparent

        DontDestroyOnLoad(flashObj);
    }

    private void OnEnable()
    {
         if (Instance != this) return;
        controls.Player.SwapEnvironment.performed += OnSwapEnvironment;
        controls.Player.Enable();
    }

    private void OnDisable()
    {
         if (Instance != this) return;
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

        // Find player references
        FindPlayerReferences();

        // Apply the current environment state
        ApplyCurrentEnvironmentState();

        // Debug.Log($"Applied environment state in new scene: Environment_{(currentIndex == 0 ? 'A' : 'B')} active");
    }

    private void FindEnvironmentsInScene()
    {
        // Find Environment_A and Environment_B in the current scene
            var roots = SceneManager.GetActiveScene()
                          .GetRootGameObjects();
    environments = new GameObject[2];
    foreach (var go in roots)
    {
        if (go.name == ENV_A_NAME)
            environments[0] = go;
        else if (go.name == ENV_B_NAME)
            environments[1] = go;
    }

    if (environments[0] == null)
        Debug.LogError($"[{nameof(EnvironmentManager)}] Could not find {ENV_A_NAME} in scene!");
    if (environments[1] == null)
        Debug.LogError($"[{nameof(EnvironmentManager)}] Could not find {ENV_B_NAME} in scene!");

        // Log what we found
        // Debug.Log($"Found environments in scene: A={envA != null}, B={envB != null}");
    }

    private void FindPlayerReferences()
    {
        // Find player animator
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerAnimator = player.GetComponent<Animator>();
            playerMovement = player.GetComponent<PlayerMovement>();

        // Debug.Log($"Found player reference - playerAnimator: {playerAnimator != null}");
    }

    private void ApplyCurrentEnvironmentState()
    {
        // Safety check
        if (environments == null || environments.Length < 2)
        {
            // Debug.LogWarning("Cannot apply environment state - environments not found");
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
        SoundManager.Instance.PlayBackgroundMusic(currentIndex == 0 ? SoundType.BACKGROUND_OW : SoundType.BACKGROUND_UW);

    }

    private void OnSwapEnvironment(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || isTransforming) return;

        // Debug.Log("Swap Environment Initiated");

        // Safety check
        if (environments == null || environments.Length < 2)
        {
            Debug.LogWarning("Cannot swap environments - environments not found");
            return;
        }

        // Start transformation sequence
        StartCoroutine(PerformEnvironmentTransition());
    }

    private IEnumerator PerformEnvironmentTransition()
    {
        if (swapAllowed)
        {
            // Set transforming flag to block input
            isTransforming = true;
            playerMovement.DisableMovement();

            // Play transformation animation if animator exists
            if (playerAnimator != null)
            {

                // Debug.Log("Playing transformation animation");
                playerAnimator.SetTrigger(TRANSFORMATION_ANIM);
                SoundManager.Instance.Play(SoundType.SWAP_CHARGE);

                // Wait until the point where we want to show the flash
                yield return new WaitForSeconds(flashDelay);


                // Play screen flash effect
                SoundManager.Instance.Play(SoundType.DIM_SWAP);
                StartCoroutine(PlayScreenFlash());

                // Wait for remaining animation time
                yield return new WaitForSeconds(transformationDuration - flashDelay);
            }
            else
            {
                Debug.LogWarning("Cannot play animation - player animator not found");
                yield return new WaitForSeconds(transformationDuration);
            }

            // Deactivate current environment
            if (environments[currentIndex] != null)
            {
                environments[currentIndex].SetActive(false);
                // Debug.Log("Deactivated Environment_" + (currentIndex == 0 ? 'A' : 'B'));
            }

            // Switch to next environment
            currentIndex = (currentIndex + 1) % environments.Length;

            // Activate new environment
            if (environments[currentIndex] != null)
            {
                environments[currentIndex].SetActive(true);
                // Debug.Log($"Switched to Environment_{(currentIndex == 0 ? 'A' : 'B')}");
            }
            SoundManager.Instance.PlayBackgroundMusic(currentIndex == 0 ? SoundType.BACKGROUND_OW : SoundType.BACKGROUND_UW); // Swaps bg music, fades out then fades back in 
            UpdateRichardState();


            // Allow input again
            playerMovement.EnableMovement();
            isTransforming = false;
        }
    }

    private void UpdateRichardState()
    {
        bool inA = (currentIndex == 0);
        if (richardGood != null) richardGood.gameObject.SetActive(inA);
        if (richardEvil != null) richardEvil.gameObject.SetActive(!inA);
        playerVariant?.SetVariant(inA);
    }

    private void FindRichardUI()
    {
        var goGood = GameObject.Find("richard");
        var goEvil = GameObject.Find("richard (evil)");

        if (goGood != null) richardGood = goGood.GetComponent<Image>();
        if (goEvil != null) richardEvil = goEvil.GetComponent<Image>();
    }
}