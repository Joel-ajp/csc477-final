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
    public Sprite icon;
    public UnityEngine.UI.Image displaySprite;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI displayCost;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    private bool flashin;

    [Header("Item info")]
    public string type;
    public int price;
    public int stat = 0;

    [Header("Reference Variables")]
    private InventoryManager inventory;
    private Coins coins;


    void Start()
    {
        displaySprite.sprite = icon;
        flashin = false;

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        coins = GameObject.FindGameObjectWithTag("UI").GetComponent<Coins>();
    }

    // Update is called once per frame
    public void changeHighlight(bool active)
    {
        if (!flashin)
        {
            if (active)
            {
                displaySprite.color = highlightColor;
                updateText();
            }
            else
            {
                displaySprite.color = normalColor;
            }
        }
    }

    public void updateText()
    {
        displayName.text = itemName;
        displayCost.text = " $" + price;
    }

    public void tryPurchase()
    {
        int curCoins = coins.CurrentCoins;
        if (price <= curCoins)
        {
            purchaseItem();
            flash(Color.green);
            coins.spendCoins(price);
        }
        else
        {
            flash(Color.red);
            Debug.Log("Not enough coins");
        }
    }

    public void flash(Color color, float duration = 0.1f)
    {
        flashin = true;
        StartCoroutine(FlashRoutine(color, duration));
    }

    private IEnumerator FlashRoutine(Color color, float duration)
    {
        displaySprite.color = color;
        yield return new WaitForSeconds(duration);
        displaySprite.color = normalColor;
        flashin = false;
    }


    public void purchaseItem()
    {
        if (type == "stat")
        {
            Debug.Log($"Upgraded stat {stat} for {price} coins!");
            inventory.updateStat(stat);
        }
        else
        {
            Debug.Log($"Purchased {itemName} for {price} coins!");
        }
    }
}
