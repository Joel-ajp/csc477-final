using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class EInteractable : MonoBehaviour
{
    public float highlightRadius = 1.5f;

    PlayerControls controls;
    bool playerInRange;

    void Awake()
    {
        // 1) Instantiate your generated input class
        controls = new PlayerControls();

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

    void OnEnable()  => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        // optional: show highlight when close
        var player = GameObject.FindWithTag("Player")?.transform;
        if (player != null)
        {
            float d = Vector2.Distance(player.position, transform.position);
            Highlight(d <= highlightRadius);
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
        // e.g. GetComponent<SpriteRenderer>().color = on ? Color.yellow : Color.white;
    }

    void DoInteract()
    {
        Debug.Log($"[E] Interacted with {name}");
        // ← your real logic here
    }
}
