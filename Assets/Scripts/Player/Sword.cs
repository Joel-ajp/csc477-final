using UnityEngine;
using UnityEngine.InputSystem;
public class Sword : MonoBehaviour
{
    [SerializeField] private readonly PlayerMovement playerMovement;
    private Animator _animator;
    private PlayerControls _controls;
    [SerializeField] private ActiveWeapon activeWeapon;
    
    // Add attack speed parameter
    [SerializeField] [Range(0.5f, 2f)] private float attackSpeed = 1f;
    
    // Reference to the sword's renderer or gameObject
    [SerializeField] private GameObject swordModel;
    
    // Track if player is currently attacking
    private bool isAttacking = false;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controls = new PlayerControls();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        
        // Hide sword initially
        if (swordModel != null)
        {
            swordModel.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Only update rotation if attacking
        if (!isAttacking)
            return;
            
        Vector2 dir = playerMovement.LastMovement;
        if (dir == Vector2.zero)
            return;
        // moving up or down or left or right
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // Face left or right flip the y
            if (dir.x < 0f)
            {
                // moving left
                activeWeapon.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                // moving right
                activeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            // Face up or down flip the z
            if (dir.y > 0)
            {
                activeWeapon.transform.localRotation = Quaternion.Euler(0,0,90f);
                activeWeapon.transform.localPosition = new Vector3(0, .1f, 0);
            }
            else
            {
                activeWeapon.transform.localRotation = Quaternion.Euler(0,0,-90f);
                activeWeapon.transform.localPosition = new Vector3(0, -.1f, 0);
            }
        }
    }
    
    private void OnEnable()
    {
        _controls.Enable();
        _controls.Player.Swing.started += _ => Attack();
    }
    
    private void OnDisable()
    {
        _controls.Player.Swing.started -= _ => Attack();
        _controls.Disable();
    }
    
    private void Attack()
    {
        // Show sword during attack
        if (swordModel != null)
        {
            swordModel.SetActive(true);
        }
        
        // Set attack state
        isAttacking = true;
        
        // Apply attack speed to animator
        _animator.SetFloat("AttackSpeed", attackSpeed);
        _animator.SetTrigger("Attack");
    }
    
    // Call this at the end of attack animation through Animation Event
    public void OnAttackEnd()
    {
        // Hide sword when attack ends
        if (swordModel != null)
        {
            swordModel.SetActive(false);
        }
        
        // Reset attack state
        isAttacking = false;
    }
}