using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Stuff for the UI")]
    public UnityEngine.UI.Image displaySprite;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI displayDesc;
    public TextMeshProUGUI displayCost;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    private bool flashin;

    [Header("Item info")]
    public string itemName;
    public string itemDesc;
    public string type;
    public int price;
    public int stat = 0;

    [Header("Reference Variables")]
    private InventoryManager inventory;
    private Coins coins;


    void Start()
    {
        flashin = false;

        inventory = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryManager>();
        coins = GameObject.FindGameObjectWithTag("Player").GetComponent<Coins>();
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
        displayDesc.text = itemDesc;
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
        SoundManager.Instance.Play(SoundType.SHOP_PURCHASE);
        if (type == "stat")
        {
            Debug.Log($"Upgraded stat {stat} for {price} coins!");
            inventory.updateStat(stat);
        }
        else if (type == "crystal")
        {
            Debug.Log($"Purchased {itemName} for {price} coins!");
            inventory.gainedCrystal();
            Destroy(this);
        }
        else
        {
            Debug.Log($"Purchased {itemName} for {price} coins!");
        }
    }
}
