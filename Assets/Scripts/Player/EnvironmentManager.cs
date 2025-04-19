using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

class EnvironmentManager : MonoBehaviour
{
    PlayerControls controls;
    public GameObject[] environments;
    int currentIndex = 0;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.SwapEnvironment.performed += OnSwapEnvironment;
        controls.Player.Enable();
        // also set your initial active environment
        for (int i = 0; i < environments.Length; i++)
            environments[i].SetActive(i == currentIndex);
    }

    void OnDisable()
    {
        controls.Player.SwapEnvironment.performed -= OnSwapEnvironment;
        controls.Player.Disable();
    }

    void OnSwapEnvironment(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        Debug.Log("Swap Environment Pressed");
        environments[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % environments.Length;
        environments[currentIndex].SetActive(true);
    }
}
