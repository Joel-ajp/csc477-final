using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighScore;

public class ScoreStarter : MonoBehaviour
{
    [SerializeField] private SubmitScore submitScore;
    private bool decayScoreActive = false;
    private Coroutine decayCoroutine;

    void Start()
    {
        Debug.Log("Started");
        HS.Init(this, "Fractured");

        if (!decayScoreActive)
        {
            SubmitScore._speedScore = 10000;
            decayCoroutine = StartCoroutine(DecayScoreOverTime());
            decayScoreActive = true;
        }
    }

    void OnDestroy()
    {
        if (decayCoroutine != null)
        {
            StopCoroutine(decayCoroutine);
        }
        decayScoreActive = false;
    }

    private IEnumerator DecayScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SubmitScore.DecreaseSpeedScore(15);  // new static method you'll add
        }
    }

}
