using UnityEngine;
using TMPro;       // for TMP_InputField
using UnityEngine.UI;  // for Button
using HighScore;   // for HS

public class SubmitScore : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;

    private void Awake()
    {
    }

    public void OnSubmitScore()
    {
        // get name (or default)
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "Anonymous";

        // pull coin total from your singleton
        int coinScore = (Coins.Instance != null) ? Coins.Instance.CurrentCoins : 0;

        // submit the score
        HS.SubmitHighScore(this, playerName, coinScore);
        Debug.Log("Score submitted!");

        // cleanup / unpause
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        Destroy(gameObject);
    }
}
