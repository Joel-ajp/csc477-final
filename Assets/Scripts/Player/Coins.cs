using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public static Coins Instance { get; private set; }

    [SerializeField] private TMP_Text coinsText;

    public int coins;

    public void Start()
    {
        UpdateUI();
    }

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
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinsText.text = coins.ToString();
    }
    public int CurrentCoins => coins;
}
