using UnityEngine;
using TMPro;       // ← for TMP_Text / TextMeshProUGUI

public class PlayerLives : MonoBehaviour
{
    [Header("Lives data")]
    public int lives = 6;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI livesText;  

    void Start()
    {
        // initialize the display at start
        UpdateLivesUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        lives--;
        Debug.Log($"Life lost! Remaining: {lives}");

        Destroy(collision.gameObject);

        // update the text field immediately
        UpdateLivesUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over");
            Destroy(gameObject);
        }
    }

    private void UpdateLivesUI()
    {
        // prefix “x” is cosmetic: matches your inspector snapshot
        livesText.text = $"x{lives}";
    }
}
