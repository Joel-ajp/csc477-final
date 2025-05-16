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
    [SerializeField] private int magazineSize = 5;
    [SerializeField] private float reloadDuration = 2f;
    [SerializeField] private float attackDuration = 0.2f; // Duration of attack animation
    
    private int _shotsRemaining;
    private bool _isReloading = false;
    private bool _isAttacking = false;
    private PlayerControls _controls;
    
    private void Awake()
    {
        _controls = new PlayerControls();
        _shotsRemaining = magazineSize;
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
    
    private void ApplyFacing(float zAngle, Vector2 offset)
    {
        transform.localEulerAngles = new Vector3(0f, 0f, zAngle);
        transform.localPosition = new Vector3(offset.x, offset.y, transform.localPosition.z);
    }
    
    private void OnSwing(InputAction.CallbackContext ctx)
    {
        // Don't allow shooting while attacking or reloading
        if (_isAttacking || _isReloading)
        {
            if (_isReloading)
            {
                Debug.Log("[Bow] Reloading – please wait.");
            }
            return;
        }
        
        if (_shotsRemaining <= 0)
        {
            StartCoroutine(ReloadRoutine());
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
        
        _shotsRemaining--;
        Debug.Log($"[Bow] Fired! {_shotsRemaining}/{magazineSize} arrows left.");
        Shoot();
        
        // Wait for the rest of the animation to complete
        yield return new WaitForSeconds(attackDuration * 0.5f);
        
        // End attack animation
        playerAnimator.SetBool("Attack", false);
        
        // Re-enable player movement
        playerMovement.EnableMovement();
        
        _isAttacking = false;
        
        if (_shotsRemaining <= 0)
            StartCoroutine(ReloadRoutine());
    }
    
    private void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.velocity = proj.transform.right * projectileSpeed;
        Destroy(proj, lifeTime);
    }
    
    private IEnumerator ReloadRoutine()
    {
        _isReloading = true;
        
        // Optional: You could trigger a reload animation here
        // playerAnimator.SetBool("Reloading", true);
        
        yield return new WaitForSeconds(reloadDuration);
        
        // Optional: End reload animation
        // playerAnimator.SetBool("Reloading", false);
        
        _shotsRemaining = magazineSize;
        _isReloading = false;
    }
}