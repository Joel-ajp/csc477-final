using System.Collections;
using System.Collections.Generic;
using HighScore;
using UnityEngine;

public class TestHighScore : MonoBehaviour
{
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        HS.Init(this, "👍");
        StartCoroutine(EndOfGame());
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator EndOfGame()
    {
        yield return new WaitForSeconds(5.0f);
        HS.SubmitHighScore(this, "☠️☠️☠️Fire☠️☠️☠️", 200);
    }
}
