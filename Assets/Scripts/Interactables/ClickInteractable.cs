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

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

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
