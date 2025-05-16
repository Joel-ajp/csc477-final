using UnityEngine;
using TMPro;       // ← for TMP_Text / TextMeshProUGUI
using HighScore;
using UnityEngine.UI;
using System.Collections;

public class PlayerLives : MonoBehaviour
{
    public int lives = 6;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private SubmitScore submitScoreObject;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    void Start()
    {
        UpdateLivesUI();
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        // You can leave Update clean if you only handle death in the coroutine
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        StartCoroutine(FlashAndDamage());
    }

    private IEnumerator FlashAndDamage()
    {
        // flash red
        spriteRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material.color = defaultColor;

        // apply damage
        lives--;
        Debug.Log($"Life lost! Remaining: {lives}");
        UpdateLivesUI();

        // if dead, game over
        if (lives <= 0)
        {
            HandleGameOver();
        }
    }

    public void gainHearts()
    {
        lives++;
        UpdateLivesUI();
    }

    private void UpdateLivesUI()
    {
        livesText.text = $"x{lives}";
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over");
        SoundManager.Instance.Play(SoundType.GAME_OVER);
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        // pass the coin score into your submit object
        int coinScore = (Coins.Instance != null) ? Coins.Instance.CurrentCoins : 0;
        submitScoreObject.SetCoinScore(coinScore);

        // destroy player sprite so it disappears
        Destroy(gameObject);
    }

    // for testing via Inspector
    public void kill() => lives = 0;
}
