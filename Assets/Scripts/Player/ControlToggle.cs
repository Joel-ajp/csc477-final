using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlToggle : MonoBehaviour
{

    public GameObject main_panel;
    public GameObject controls_panel;
    private bool controls_open;


    // Start is called before the first frame update
    void Start()
    {
        main_panel.SetActive(true);
        controls_panel.SetActive(false);
        controls_open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleControls();
        }
    }

    public void ToggleControls()
    {
        controls_open = !controls_open;
        main_panel.SetActive(!controls_open);
        controls_panel.SetActive(controls_open);
    }
}
