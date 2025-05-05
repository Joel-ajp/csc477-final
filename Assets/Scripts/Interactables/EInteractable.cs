using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class EInteractable : MonoBehaviour
{
    public float highlightRadius = 1.5f;

    PlayerControls controls;
    bool playerInRange;
    GameObject exclamationHighlight;

    void Awake()
    {
        // 1) Instantiate your generated input class
        controls = ControlsManager.Instance.Controls; // Gets the input class

        // 2) Hook the E‑key action
        controls.Player.InteractE.performed += ctx =>
        {
            if (playerInRange)
                DoInteract();
        };

        // ensure your collider is a trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // void OnEnable() => controls.Enable();
    // void OnDisable() => controls.Disable();

    void Update()
    {
        // optional: show highlight when close
        var player = GameObject.FindWithTag("Player")?.transform;
        if (player != null)
        {

            float d = Vector2.Distance(player.position, transform.position);

            Highlight(d <= highlightRadius);
        }
        else
        {
            Debug.Log("Thats Wierd");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Highlight(bool on)
    {
        GameObject exPrefab = Resources.Load<GameObject>("Exclamation");


        if (on && exclamationHighlight == null) // if its on and an instance doesnt exist already
        {
            Vector3 worldPosition = transform.position + Vector3.up * 1f;
            exclamationHighlight = Instantiate(exPrefab, worldPosition * 1f, Quaternion.identity);

        }
        else if (!on && exclamationHighlight != null) // if its off and a instance exists
        {
            Destroy(exclamationHighlight);
        }

    }

    protected virtual void DoInteract()
    {
        // Made it protected virtual so that the function can be modified/overriden in child functions
        // See lever for example
        Debug.Log($"[E] Interacted with {name}");
        // ← your real logic here
    }
}
