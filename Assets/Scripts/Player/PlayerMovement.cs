using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed = 5f;
    public static PlayerMovement Instance;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
    private Vector2 _lastMovement = Vector2.down;
    public Vector2 LastMovement => _lastMovement;
    public PlayerStats stats;
    private float speedModifier;
    
    // Property to store the position that should be set after scene load
    public static Vector2 NextSpawnPosition { get; set; }
    private static bool _shouldSetPosition = false;

    // Added flag to control whether player movement is enabled
    private bool _movementEnabled = true;
    
    // Public property to check if movement is enabled
    public bool MovementEnabled => _movementEnabled;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // Make sure this object is tagged as Player
        gameObject.tag = "Player";
        DontDestroyOnLoad(gameObject);
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        // Register for scene load events to handle position setting
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Unregister to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Always force update camera references on scene load
        StartCoroutine(ForceUpdateAllCameras());
        
        if (_shouldSetPosition)
        {
            transform.position = NextSpawnPosition;
            _shouldSetPosition = false;
            Debug.Log($"PlayerMovement: Set player position to {NextSpawnPosition} after scene load");
        }
    }

    private IEnumerator ForceUpdateAllCameras()
    {
        // Wait a couple of frames to ensure all scene objects are initialized
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        Debug.Log("Attempting to update cameras after scene load");
        
        // Try to find cameras by component directly (most reliable)
        var allVCams = Object.FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>(true);
        foreach (var vcam in allVCams)
        {
            vcam.Follow = transform;
            vcam.PreviousStateIsValid = false;
            Debug.Log("Updated camera (by component): " + vcam.name);
        }
        
        // Try both potential camera tag formats as backup
        UpdateCamerasByTag("VirtualCamera");
        
        // Find CinemachineBrain and force update
        var brains = Object.FindObjectsOfType<Cinemachine.CinemachineBrain>(true);
        foreach (var brain in brains)
        {
            brain.enabled = false;
            brain.enabled = true;
            Debug.Log("Reset CinemachineBrain: " + brain.name);
        }
    }
   
    private void UpdateCamerasByTag(string tag)
    {
        var vcams = GameObject.FindGameObjectsWithTag(tag);
        Debug.Log($"Found {vcams.Length} cameras with tag '{tag}'");

        foreach (var vcamObj in vcams)
        {
            var vcam = vcamObj.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = transform;
                vcam.PreviousStateIsValid = false;
                Debug.Log($"Set camera {vcamObj.name} to follow player");
            }
        }
    }
    
    // Public method to set the next spawn position before loading a new scene
    public static void SetNextSpawnPosition(Vector2 position)
    {
        NextSpawnPosition = position;
        _shouldSetPosition = true;
        Debug.Log($"PlayerMovement: Set next spawn position to {position}");
    }

    // Enable player movement
    public void EnableMovement()
    {
        _movementEnabled = true;
        Debug.Log("Player movement enabled");
    }

    // Disable player movement
    public void DisableMovement()
    {
        _movementEnabled = false;
        // Immediately stop any current movement
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }
        // Reset animator parameters to idle state
        if (_animator != null)
        {
            _animator.SetFloat(_horizontal, 0);
            _animator.SetFloat(_vertical, 0);
        }
        Debug.Log("Player movement disabled");
    }

    // Update is called once per frame
    void Update()
    {
        // Only process movement if it's enabled
        if (_movementEnabled)
        {
            speedModifier = (stats.movement_speed - 1) * 0.2f;

            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
            _rb.velocity = _movement * (_moveSpeed + speedModifier);
            _animator.SetFloat(_horizontal, _movement.x);
            _animator.SetFloat(_vertical, _movement.y);
            
            if (_movement != Vector2.zero)
            {
                _lastMovement = _movement;
                _animator.SetFloat(_lastHorizontal, _movement.x);
                _animator.SetFloat(_lastVertical, _movement.y);
            }
        }
        else
        {
            // Ensure velocity is zero when movement is disabled
            if (_rb.velocity != Vector2.zero)
            {
                _rb.velocity = Vector2.zero;
            }
        }
    }
}