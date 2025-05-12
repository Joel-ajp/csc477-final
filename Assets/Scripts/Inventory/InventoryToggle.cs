using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public GameObject panel;
    public RectTransform button;

    private bool isOpen;

    private Vector3 closePos = new Vector3(-70, 70, 0);
    private Vector3 openPos = new Vector3(-70, 375, 0);

    void Start()
    {
        panel.SetActive(false);
        button.anchoredPosition = closePos;
        isOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }


    public void ToggleInventory()
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);

        if (isOpen)
        {
            button.anchoredPosition = openPos;
        }
        else
        {
            button.anchoredPosition = closePos;
        }
    }
}
