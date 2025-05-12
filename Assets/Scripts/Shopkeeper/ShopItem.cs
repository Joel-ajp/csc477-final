using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Stuff for the UI")]
    public string itemName;
    public string price;
    public Sprite icon;
    public UnityEngine.UI.Image displaySprite;
    public TextMeshProUGUI displayText;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    [Header("Item info")]
    public string type;
    public int stat = 0;

    [Header("Reference variables")]
    public GameObject inventory;


    void Start()
    {
        displayText.text = itemName + " $" + price;
        displaySprite.sprite = icon;
    }

    // Update is called once per frame
    public void changeHighlight(bool active)
    {
        displaySprite.color = active ? highlightColor : normalColor;
    }

    public void purchaseItem()
    {
        if (type == "stat")
        {
            Debug.Log($"Upgraded stat {stat} for {price} coins!");
            inventory.GetComponent<InventoryManager>().updateStat(stat);
        }
        else
        {
            Debug.Log($"Purchased {itemName} for {price} coins!");
        }
    }
}
