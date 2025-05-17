using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Bow : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 upOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private Vector2 downOffset;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float attackDuration = 0.2f; // Duration of attack animation
    
    // Mana system parameters
    [Header("Mana System")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana = 100f;
    [SerializeField] private float manaPerShot = 20f;
    [SerializeField] private float manaRegenRate = 10f; // Mana regained per second
    [SerializeField] private float manaRegenDelay = 1f; // Delay before mana starts regenerating after shooting
    
    private bool _isAttacking = false;
    private bool _canRegenMana = true;
    private float _lastShotTime = -999f;
    private PlayerControls _controls;
    
    private void Awake()
    {
        _controls = new PlayerControls();
        currentMana = maxMana;
    }
    
    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Swing.started += OnSwing;
    }
    
    private void OnDisable()
    {
        _controls.Player.Swing.started -= OnSwing;
        _controls.Disable();
    }
    
    private void Update()
    {
        UpdateFacing();
        RegenMana();
    }
    
    private void UpdateFacing()
    {
        Vector2 dir = playerMovement.LastMovement;
        if (dir == Vector2.zero) return;
        
        // choose facing & offset
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0f) {
                ApplyFacing(0f, rightOffset);
            }
            else {
                ApplyFacing(180f, leftOffset);
            }
        }
        else
        {
            if (dir.y > 0f) {
                ApplyFacing(90f, upOffset);
            }
            else {
                ApplyFacing(-90f, downOffset);
            }
        }
    }
    
    private void RegenMana()
    {
        // Check if we should start regenerating mana
        if (Time.time > _lastShotTime + manaRegenDelay)
        {
            _canRegenMana = true;
        }
        
        // Regenerate mana when not shooting
        if (_canRegenMana && currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, maxMana);
        }
    }
    
    private void ApplyFacing(float zAngle, Vector2 offset)
    {
        transform.localEulerAngles = new Vector3(0f, 0f, zAngle);
        transform.localPosition = new Vector3(offset.x, offset.y, transform.localPosition.z);
    }
    
    private void OnSwing(InputAction.CallbackContext ctx)
    {
        // Don't allow shooting while attacking
        if (_isAttacking)
        {
            return;
        }
        
        // Check if we have enough mana
        if (currentMana < manaPerShot)
        {
            Debug.Log($"[Bow] Not enough mana! Current: {currentMana:F1}/{maxMana}");
            return;
        }
        
        // Start the attack
        StartCoroutine(AttackRoutine());
    }
    
    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        
        // Disable player movement
        playerMovement.DisableMovement();
        
        // Trigger attack animation
        playerAnimator.SetBool("Attack", true);
        
        // Fire the arrow midway through the animation
        yield return new WaitForSeconds(attackDuration * 0.5f);
        
        // Consume mana
        currentMana -= manaPerShot;
        _lastShotTime = Time.time;
        _canRegenMana = false;
        
        Debug.Log($"[Bow] Fired! Mana: {currentMana:F1}/{maxMana}");
        Shoot();
        
        // Wait for the rest of the animation to complete
        yield return new WaitForSeconds(attackDuration * 0.5f);
        
        // End attack animation
        playerAnimator.SetBool("Attack", false);
        
        // Re-enable player movement
        playerMovement.EnableMovement();
        
        _isAttacking = false;
    }
    
    private void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.velocity = proj.transform.right * projectileSpeed;
        Destroy(proj, lifeTime);
    }
    
    // Public getter for UI to display mana
    public float GetCurrentMana()
    {
        return currentMana;
    }
    
    public float GetMaxMana()
    {
        return maxMana;
    }
    
    // Optional: UI helper method to get normalized mana (0-1)
    public float GetManaPercentage()
    {
        return currentMana / maxMana;
    }
}