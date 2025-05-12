using System.Collections;
using System.Collections.Generic;
using HighScore;
using UnityEngine;

public class HighScoreManger : MonoBehaviour
{
    private int _score = 0;

    // Made this before seeing player lives already has it. 
    // If we want to use this instead it may be better as we can do points based off this
    // So we can have points added per crystal taken
    // But current, This is not used.
    void Start()
    {
        HS.Init(this, "Fractured");
    }

    public void increaseScore(int num)
    {
        _score += num;
    }

    public void decreaseScore(int num)
    {
        _score -= num;
    }

    public IEnumerator EndGame(string name)
    {
        Debug.Log("Game Over, Uploading Score");
        yield return new WaitForSeconds(5.0f);
        HS.SubmitHighScore(this, name, _score);
    }
}
