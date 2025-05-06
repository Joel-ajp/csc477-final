using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public static Coins Instance { get; private set; }

    [SerializeField] private TMP_Text coinsText;  
    private int coins;

    private void Awake()
    {
        /*if (Instance != null && Instance != this){
        Destroy(gameObject);
        return;
        }*/
        Instance = this;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    public void spendCoins(int amount)
    {
        if (amount > coins)
        {
            Debug.Log("not enough coins");
        }
        else if (amount <= coins)
        {
            Debug.Log(amount + " coin(s) have been spent");
            coins -= amount;
            // add code here for what to do when a thing is bought
            // if a crystal was bought then call the 
            // InventoryManager public function "gainedCrystal" any other
            // pickup has to be added manually (talk to Gavin)
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinsText.text = coins.ToString();
    }
}
