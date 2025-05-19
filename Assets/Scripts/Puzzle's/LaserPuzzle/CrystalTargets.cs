using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CrystalTargets : MonoBehaviour
{

    public bool isActive = false;
    private Light2D _activeLight;
    public GameObject Pillar;

    public Sprite redSprite;       // Sprite when activated
    public Sprite whiteSprite;  // Sprite when deactivated
    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = isActive ? redSprite : whiteSprite;
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            _spriteRenderer.sprite = redSprite;
            AddLight();
            Pillar.GetComponent<LightPillar>().SetActive(true);

        }
    }

    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            _spriteRenderer.sprite = whiteSprite;
            RemoveLight();
            Pillar.GetComponent<LightPillar>().SetActive(false);
        }
    }

    private void AddLight()
    {
        if (_activeLight == null)
        {
            GameObject lightObj = new GameObject("CrystalLight");
            lightObj.transform.parent = transform;
            lightObj.transform.localPosition = Vector3.zero;

            _activeLight = lightObj.AddComponent<Light2D>();
            _activeLight.lightType = Light2D.LightType.Point;
            _activeLight.color = Color.red;
            _activeLight.intensity = .5f;
            _activeLight.pointLightOuterRadius = 2f;
        }
    }

    private void RemoveLight()
    {
        if (_activeLight != null)
        {
            Destroy(_activeLight.gameObject);
            _activeLight = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
