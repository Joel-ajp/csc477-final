using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariant : MonoBehaviour
{
    public Sprite goodSprite;
    public Sprite evilSprite;
    public RuntimeAnimatorController goodController;
    public RuntimeAnimatorController evilController;

    private SpriteRenderer _sr;
    private Animator       _anim;

    void Awake()
    {
        _sr   = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    public void SetVariant(bool good)
    {
        // swap the image
        _sr.sprite = good ? goodSprite : evilSprite;

        // swap the animation controller
        _anim.runtimeAnimatorController =
            good ? goodController : evilController;
    }
}
