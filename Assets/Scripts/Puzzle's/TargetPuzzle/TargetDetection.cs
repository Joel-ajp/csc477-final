using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    public Sprite activatedPillarSprite;
    public GameObject targetPillar;
    public bool Timed;
    public int Timed_Length;
    private Sprite _deactivatedSprite;

    public bool activated { get; private set; } = false;

    void Start()
    {
        SpriteRenderer sr = targetPillar.GetComponent<SpriteRenderer>();
        _deactivatedSprite = sr.sprite;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated)
        {
            activated = true;

            if (targetPillar != null)
            {
                SpriteRenderer sr = targetPillar.GetComponent<SpriteRenderer>();
                if (sr != null && activatedPillarSprite != null)
                {
                    sr.sprite = activatedPillarSprite;
                }


            }
        }
        SoundManager.Instance.Play(SoundType.FIRE_HIT, -1f, .5f);
        Destroy(other.gameObject);

        if (Timed)
        {
            StartCoroutine(ResetAfterDelay());
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(Timed_Length);

        if (targetPillar != null)
        {
            SpriteRenderer sr = targetPillar.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = _deactivatedSprite;
            }
        }

        activated = false;
    }
}
