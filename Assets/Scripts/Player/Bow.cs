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

    [SerializeField] private int magazineSize  = 5;
    [SerializeField] private float reloadDuration = 2f;

    private int _shotsRemaining;
    private bool _isReloading = false;
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
                ApplyFacing(  0f, rightOffset);
            }
            else{
                ApplyFacing(180f, leftOffset);
            }
        }
        else
        {
            if (dir.y > 0f){
                ApplyFacing( 90f, upOffset);
            }
            else{
                ApplyFacing(-90f, downOffset);
            };
        }
    }

    private void ApplyFacing(float zAngle, Vector2 offset)
    {
        transform.localEulerAngles = new Vector3(0f, 0f, zAngle);
        transform.localPosition = new Vector3(offset.x, offset.y, transform.localPosition.z);
    }

     private void OnSwing(InputAction.CallbackContext ctx)
    {
        if (_isReloading)
        {
            //we could play an animation or something in the ui for reloading
            Debug.Log("[Bow] Reloading – please wait.");
            return;
        }

        if (_shotsRemaining <= 0)
        {
            StartCoroutine(ReloadRoutine());
            return;
        }

        _shotsRemaining--;
        Debug.Log($"[Bow] Fired! {_shotsRemaining}/{magazineSize} arrows left.");

        Shoot();

        if (_shotsRemaining <= 0)
            StartCoroutine(ReloadRoutine());
    }

    private void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

        //we can change the hard corded projectile speed to the shop bought one
        //also for lifetime they stay on the screen longer which can be adjusted by a stat
        rb.velocity = proj.transform.right * projectileSpeed;
        Destroy(proj, lifeTime);
    }
      private IEnumerator ReloadRoutine()
    {
        _isReloading = true;
        yield return new WaitForSeconds(reloadDuration);
        _shotsRemaining = magazineSize;
        _isReloading  = false;
    }
}
