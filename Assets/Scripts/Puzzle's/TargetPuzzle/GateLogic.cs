using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLogic : MonoBehaviour
{
    public List<GameObject> targetDetections;
    public Sprite openSprite;
    private SpriteRenderer sr;
    private Collider2D col;
    private bool gateOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkActive() && !gateOpened)
        {
            gateOpened = true;
            SoundManager.Instance.Play(SoundType.GATE_OPEN, -1f, .5f);
            SoundManager.Instance.Play(SoundType.SUCCESS, 1f, .5f);

            if (sr != null)
            {
                sr.sprite = openSprite;
            }

            if (col != null)
            {
                col.enabled = false;
            }
        }
    }


    bool checkActive()
    {
        foreach (GameObject target in targetDetections)
        {
            TargetDetection targetScript = target.GetComponent<TargetDetection>();
            if (!targetScript.activated || targetScript == null)
            {
                return false;
            }
        }
        return true;
    }
}
