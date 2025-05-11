using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeeper : MonoBehaviour
{
    public GameObject keeperUI;
    public GameObject overworld;
    public GameObject underworld;
    public Image overKeep;
    public Image underKeep;

    public List<ShopItem> shopItems;

    private bool playerCanShop;
    private bool isOpen;
    private int index = 0;

    void Start()
    {
        playerCanShop = false;
        isOpen = false;
        keeperUI.SetActive(false);
        UpdateHighlight();
    }

    void Update()
    {
        if (playerCanShop && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }

        if (isOpen)
        {
            ShopNavigation();
        }

        // Check environment
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

    void ShopNavigation()
    {
        int tempIndex = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempIndex = Mathf.Min(index + 1, shopItems.Count - 1);
            if (shopItems[tempIndex] != null)
            {
                index = tempIndex;
            }
            UpdateHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempIndex = Mathf.Max(index - 1, 0);
            if (shopItems[tempIndex] != null)
            {
                index = tempIndex;
            }
            UpdateHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            purchaseItem();
        }
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i] != null)
            {
                shopItems[i].changeHighlight(i == index);
            }
        }
    }

    public void ToggleShop()
    {
        isOpen = !isOpen;
        keeperUI.SetActive(isOpen);
        if (isOpen)
            UpdateHighlight();
    }

    public void purchaseItem()
    {
        shopItems[index].purchaseItem();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player shopping");
            playerCanShop = true;
            isOpen = true;
            UpdateHighlight();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("player done shopping");
            playerCanShop = false;
            isOpen = false;
            keeperUI.SetActive(false);
        }
    }
}

