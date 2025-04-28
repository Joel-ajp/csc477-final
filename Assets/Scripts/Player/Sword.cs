using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private Animator _animator;
    private PlayerControls  _controls;
    [SerializeField]  private ActiveWeapon activeWeapon;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controls = new PlayerControls();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        
    }

   private void Update()
{
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
            activeWeapon.transform.localRotation = Quaternion.Euler(0f,   0f, 0f);
        }
    }
    else
    {
        // Face up or down flip the z
        if (dir.y > 0f)
        {
            // moving up
            activeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f,  90f);
        }
        else
        {
            // moving down
            activeWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        }
    }
}





    private void OnEnable()
    {
        //from youtube tutorial
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
        _animator.SetTrigger("Attack");
    }

}
