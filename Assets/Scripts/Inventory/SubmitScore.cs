using UnityEngine;
using TMPro;       // for TMP_InputField
using UnityEngine.UI;  // for Button
using HighScore;
using System;   // for HS
using UnityEngine.SceneManagement;
using System.Collections;

public class SubmitScore : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    private static int _score;
    public static int _speedScore = 10000; // Starting score

    private bool _isSubmitted = false;

    public void OnSubmitScore()
    {
        // get name (or default)
        // Debug.Log("I got this far");
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
            playerName = "Anonymous";

        if (_isSubmitted == false)
        {
            _score += _speedScore; // add in the speedscore
            HS.SubmitHighScore(this, playerName, _score);
            _isSubmitted = true;
        }

        Debug.Log("Score submitted!" + _score.ToString() + _speedScore.ToString());

        // cleanup / unpause
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        Reset();
        SceneManager.LoadScene("Main Menu rahhh");

        Destroy(gameObject);
    }

    void Reset()
    {
        _speedScore = 10000;
        _score = 0;
        _isSubmitted = false;
    }

    public static void increaseScore(int num)
    {
        _score += num;
        Debug.Log(_score);
    }

    public static void decreaseScore(int num)
    {
        _score -= num;
    }

    public static void DecreaseSpeedScore(int amount)
    {
        _speedScore = Mathf.Max(0, _speedScore - amount);
    }

    public static int getReturnScore()
    {
        return _score + _speedScore;
    }

    public static void SetCoinScore(int coin)
    {
        _score += coin;
    }
}
