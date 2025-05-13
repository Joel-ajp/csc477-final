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

        // hide the panel until game over
        gameOverPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        lives--;
        Debug.Log($"Life lost! Remaining: {lives}");

        // update the text field immediately
        UpdateLivesUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over");
            OnGameOver();
            Destroy(gameObject);
        }
    }

    public void UpdateLivesUI()
    {
        // prefix “x” is cosmetic: matches your inspector snapshot
        livesText.text = $"x{lives}";
    }

    public void OnGameOver()
    {
        // stop the action
        SoundManager.Instance.Play(SoundType.GAME_OVER);
        Time.timeScale = 0f;

        // show input panel
        gameOverPanel.SetActive(true);
    }


}
