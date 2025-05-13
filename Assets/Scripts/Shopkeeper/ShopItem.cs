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
    public TextMeshProUGUI displayText;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    private bool flashin;

    [Header("Item info")]
    public string type;
    public int price;
    public int stat = 0;

    [Header("Reference Variables")]
    public GameObject inventory;
    public GameObject coins;


    void Start()
    {
        displayText.text = itemName + " $" + price;
        displaySprite.sprite = icon;
        flashin = false;
    }

    // Update is called once per frame
    public void changeHighlight(bool active)
    {
        if (!flashin)
        {
            displaySprite.color = active ? highlightColor : normalColor;
        }
    }

    public void tryPurchase()
    {
        int curCoins = coins.GetComponent<Coins>().CurrentCoins;
        if (price <= curCoins)
        {
            purchaseItem();
            flash(Color.green);
            coins.GetComponent<Coins>().spendCoins(price);
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
            inventory.GetComponent<InventoryManager>().updateStat(stat);
        }
        else
        {
            Debug.Log($"Purchased {itemName} for {price} coins!");
        }
    }
}
