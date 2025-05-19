using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPillar : MonoBehaviour
{
    public Sprite ActiveSprite;
    public Sprite InactiveSprite;

    private SpriteRenderer _renderer;
    private Light2D _light;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _light = GetComponentInChildren<Light2D>();
    }

    public void SetActive(bool active)
    {
        _renderer.sprite = active ? ActiveSprite : InactiveSprite;
        _light.color = active ? Color.green : Color.red;
    }
}
