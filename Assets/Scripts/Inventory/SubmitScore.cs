using UnityEngine;
using TMPro;       // for TMP_InputField
using UnityEngine.UI;  // for Button
using HighScore;
using System;   // for HS

public class SubmitScore : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    private int _coinScore = 0;
    private bool _isSubmitted = false;

    private void Awake()
    {
        HS.Init(this, "Fractured3");
        submitButton.onClick.AddListener(OnSubmitScore);
    }

    public void OnSubmitScore()
    {
        // get name (or default)
        // Debug.Log("I got this far");
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "Anonymous";

        // pull coin total from your singleton
        int coinScore = _coinScore;

        // submit the score
        // Debug.Log(coinScore);
        if (_isSubmitted == false)
        {
            HS.SubmitHighScore(this, playerName, coinScore);
            _isSubmitted = true;
        }

        Debug.Log("Score submitted!");

        // cleanup / unpause
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        Destroy(gameObject);
    }

    public void SetCoinScore(int coin)
    {
        _coinScore = coin;
    }
}
