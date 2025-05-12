using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ClickInteractable : MonoBehaviour
{
    PlayerControls controls;
    Collider2D col;

    void Awake()
    {
        controls = ControlsManager.Instance.Controls;
        col = GetComponent<Collider2D>();
        col.isTrigger = false; // so OverlapPoint works on graphic area

        // Hook the mouse‑click action
        controls.Player.InteractClick.performed += ctx => HandleClick();
    }

    // This is what had me change so many things, this hurts, i was destroying crystal and it disabled the controls 
    // from the controlsmanager which manages all inputs. SO yeah, 90 minutes wasted. Bad coding practice to comment this
    // but im tired and leave this as a warning, please dont redo this. thanks
    // void OnEnable() => controls.Enable();
    // void OnDisable() => controls.Disable();

    void HandleClick()
    {
        // read mouse pos from the new Input System
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mouseScreen);
        Vector2 clickPoint = worldPoint;

        // if this click landed on _this_ collider, fire
        if (col.OverlapPoint(clickPoint))
        {
            DoInteract();
        }
    }

    void DoInteract()
    {
        Debug.Log($"[Click] Interacted with {name}");
        // ← your real logic here
    }
}
