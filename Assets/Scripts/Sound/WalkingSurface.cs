using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WalkingSurface : MonoBehaviour
{
    // Im getting the parent to see if its moving
    private Rigidbody2D _parentRB;

    // Only set to public currently for testing purposes, intended to be private
    public GroundSurfaceState _currentSurface;
    private SoundType _currentSurfaceSound;
    private float _pitchSpeed;
    private bool _walking;



    // Start is called before the first frame update
    void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody2D>();
        SetSurface(_currentSurface);
    }

    // Update is called once per frame
    void Update()
    {
        // This is a roundabout way of doing it, didnt want to mess with movement script yet, this can be improved later
        if (_parentRB.velocity.magnitude > 0.05f && !_walking)
        {
            StartCoroutine(PlayWalkSound());
        }
    }

    IEnumerator PlayWalkSound()
    {
        Debug.Log("Im Playing A Sound!");
        _walking = true;



        AudioClip clip = SoundManager.Instance.GetClip(_currentSurfaceSound);
        if (clip != null)
        {
            // Makes sure that the wait time is based off the proper speed
            float clipLengthAdjust = clip.length / _pitchSpeed;

            SoundManager.Instance.Play(_currentSurfaceSound, _pitchSpeed);
            yield return new WaitForSeconds(clipLengthAdjust);
        }

        _walking = false;
    }

    void SetSurface(GroundSurfaceState state)
    {
        _currentSurface = state;
        // Pitch is currently being used to match speed with animation
        switch (state)
        {
            case GroundSurfaceState.CONCRETE:
                _currentSurfaceSound = SoundType.WALK_CONCRETE;
                _pitchSpeed = 1.75f;
                break;
            case GroundSurfaceState.WOOD:
                _currentSurfaceSound = SoundType.WALK_WOOD;
                _pitchSpeed = 1.25f;
                break;
            case GroundSurfaceState.METAL:
                _currentSurfaceSound = SoundType.WALK_METAL;
                _pitchSpeed = 1.3f;
                break;
            case GroundSurfaceState.SAND:
                _currentSurfaceSound = SoundType.WALK_SAND;
                _pitchSpeed = 1.4f;
                break;
        }
    }
}
