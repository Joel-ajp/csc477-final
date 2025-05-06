using UnityEngine;
using TMPro;       // ← for TMP_Text / TextMeshProUGUI
using HighScore;
using UnityEngine.UI;  

public class PlayerLives : MonoBehaviour
{
    [Header("Lives data")]
    public int lives = 6;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI livesText;  
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;

    void Start()
    {
        // initialize the display at start
        HS.Init(this, "Fractured");
        UpdateLivesUI();
        submitButton.onClick.AddListener(OnSubmitScore);

        // hide the panel until game over
        gameOverPanel.SetActive(false);
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
            OnGameOver();
            Destroy(gameObject);
        }
    }

    private void UpdateLivesUI()
    {
        // prefix “x” is cosmetic: matches your inspector snapshot
        livesText.text = $"x{lives}";
    }

    private void OnGameOver()
    {
        // stop the action
        Time.timeScale = 0f;

        // show input panel
        gameOverPanel.SetActive(true);
    }

        private void OnSubmitScore()
    {
        // get name (or default)
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "Anonymous";

        // pull coin total from your singleton
        int coinScore = (Coins.Instance != null) ? Coins.Instance.CurrentCoins : 0;

        // submit the score
        HS.SubmitHighScore(this, playerName, coinScore);
        print("Game Over");

        // cleanup / unpause
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        Destroy(gameObject);
    }
}
