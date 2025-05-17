using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shopkeeper : MonoBehaviour
{
    [Header("Stuff for the UI")]
    public GameObject keeperUI;
    public GameObject overworld;
    public GameObject underworld;
    public GameObject dialogue;
    public Image overKeep;
    public Image underKeep;
    public TextMeshProUGUI MS;
    public TextMeshProUGUI AS;
    public TextMeshProUGUI AD;
    public TextMeshProUGUI HB;

    [Header("Reference Variables")]
    public List<ShopItem> shopItems;

    private bool isOpen;
    private int index = 0;
    private int columns = 3;
    private GameObject player;
    private PlayerStats stats;

    void Start()
    {
        isOpen = false;
        keeperUI.SetActive(false);
        UpdateHighlight();
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!isOpen && Input.GetKeyDown(KeyCode.E))
        {
            //open dialogue
            dialogue.SetActive(true);
        }
        else if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //close shop
            ToggleShop();
        }

        if (isOpen)
        {
            GetPlayerStats();
            UpdateHighlight();
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

    void GetPlayerStats()
    {
        MS.text = "lvl " + stats.movement_speed.ToString();
        AS.text = "lvl " + stats.attack_speed.ToString();
        AD.text = "lvl " + stats.attack_damage.ToString();
        HB.text = "lvl " + stats.defense.ToString();
    }

    void ShopNavigation()
    {
        int tempIndex = index;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempIndex = index + 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempIndex = index - 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempIndex = index + columns;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempIndex = index - columns;
        }

        // Clamp index within bounds and check for null
        if (tempIndex >= 0 && tempIndex < shopItems.Count && shopItems[tempIndex] != null)
        {
            index = tempIndex;
            UpdateHighlight();
        }

        if (Input.GetKeyDown(KeyCode.Return))
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
        index = 0;
        keeperUI.SetActive(isOpen);
        if (isOpen)
            UpdateHighlight();
    }

    public void purchaseItem()
    {
        shopItems[index].tryPurchase();
    }
}

