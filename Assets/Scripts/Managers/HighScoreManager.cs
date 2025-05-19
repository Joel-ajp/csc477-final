using System.Collections;
using System.Collections.Generic;
using HighScore;
using UnityEngine;

public class HighScoreManger : MonoBehaviour
{
    private static int _score = 0;
    private static int _speedScore = 10000; // Starting score
    private int decayRate = 15; // Score lost per second

    private Coroutine decayCoroutine;

    void Start()
    {
        HS.Init(this, "Fractured");
        decayCoroutine = StartCoroutine(DecayScoreOverTime());
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

    private IEnumerator DecayScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _speedScore = Mathf.Max(0, _speedScore - decayRate);
        }
    }

    public IEnumerator EndGame(string name)
    {
        Debug.Log("Game Over, Uploading Score");
        _score += _speedScore; // adds in the score for completion time
        yield return new WaitForSeconds(5.0f);
        HS.SubmitHighScore(this, name, _score);
    }
}
