using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationBob : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 1f;

    // A temporary bob animation for the exclamation prefab for interactable highlight

    private Vector3 _pos;
    // Start is called before the first frame update
    void Start()
    {
        _pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = _pos + new Vector3(0, yOffset, 0);
    }
}
