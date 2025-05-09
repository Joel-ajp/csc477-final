using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeeper : MonoBehaviour
{
    public GameObject keeperUI;
    public GameObject overworld;
    public GameObject underworld;
    public Image overKeep;
    public Image underKeep;
    
    private bool playerCanShop;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        playerCanShop = false;
        isOpen = false;
        keeperUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // allow for a keypress to open and close the shop
        if (playerCanShop)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleShop();
            }
        }

        // check which environment is open to determine which sprite to load
        if (overworld.activeInHierarchy)
        {
            overKeep.enabled = true;
            underKeep.enabled = false;
        }
        else if (underworld.activeInHierarchy)
        {
            overKeep.enabled = false;
            underKeep.enabled = true;
        }

    }

    public void ToggleShop()
    {
        isOpen = !isOpen;
        keeperUI.SetActive(isOpen);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player shopping");
            playerCanShop = true;
            isOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("player done shopping");
            playerCanShop = false;
            isOpen = true;
            keeperUI.SetActive(false);
        }
    }
}
