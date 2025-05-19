using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Sprite openSprite;
    public Sprite closedSprite;
    private SpriteRenderer _sr;
    private Collider2D _col;
    private bool gateOpened = false;



    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
    }


    public void ToggleGate()
    {
        gateOpened = !gateOpened;
        if (gateOpened)
        {
            _sr.sprite = openSprite;
            _col.enabled = false;
        }
        else
        {
            _sr.sprite = closedSprite;
            _col.enabled = true;
        }

        try
        {
            SoundManager.Instance.Play(SoundType.GATE_OPEN, -1f, .5f);
        }
        catch
        {
            // it can only play while player is in the room. Im just adding this because im lazy instead of fixing it properly
        }

    }
}
