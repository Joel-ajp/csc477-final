using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public static Coins Instance { get; private set; }

    [SerializeField] private TMP_Text coinsText;  
    private int coins;

    private void Awake()
    {
        if (Instance != null && Instance != this){
        Destroy(gameObject);
        return;
        }
        Instance = this;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinsText.text = coins.ToString();
    }
}
