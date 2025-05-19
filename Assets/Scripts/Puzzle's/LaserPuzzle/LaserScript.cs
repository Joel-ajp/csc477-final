using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private int _maxReflections = 10;
    private float _maxDistance = 100f;
    private LineRenderer _lineRenderer;
    public bool isActive;
    public Transform Barrel;
    public LayerMask foreground;
    public GameObject crystalPrefab;
    public AudioClip laserSound;
    private AudioSource _audioSource;

    private HashSet<CrystalTargets> crystalsHitThisFrame = new HashSet<CrystalTargets>();
    private HashSet<CrystalTargets> crystalsHitLastFrame = new HashSet<CrystalTargets>();

    void Start()
    {
        _lineRenderer = Barrel.GetComponent<LineRenderer>();
        _lineRenderer.alignment = LineAlignment.View;

        // makes the laser auddio source for buzzing
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = laserSound;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
        _audioSource.volume = 0.25f;

        if (isActive) // inital case
        {
            _audioSource.Play();
        }
    }

    void Update()
    {
        if (isActive)
        {
            DrawLaser(Barrel.position, Barrel.right);
        }
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        _lineRenderer.enabled = isActive;

        if (isActive)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }

    }

    void DrawLaser(Vector2 startPos, Vector2 direction)
    {
        Vector2 currentPosition = startPos;
        Vector2 currentDirection = direction;
        const float skinWidth = 0.01f;

        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, currentPosition);

        int reflections = 0;
        crystalsHitThisFrame.Clear();

        while (reflections < _maxReflections)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, _maxDistance, foreground);

            if (hit.collider != null)
            {

                if (hit.collider.name == "TargetCrystal")
                {
                    CrystalTargets crystal = hit.collider.GetComponent<CrystalTargets>();
                    if (crystal != null)
                    {
                        crystalsHitThisFrame.Add(crystal);
                        crystal.Activate();
                    }

                    // Debug.Log("going through TargetCrystal");
                    currentPosition = hit.point + currentDirection * skinWidth;
                    continue;
                }


                if (hit.collider.name == "LaserHitPoint")
                {
                    _lineRenderer.enabled.Equals(false);

                    try
                    {
                        SoundManager.Instance.Play(SoundType.SUCCESS, 1f, .5f);
                    }
                    catch
                    {
                        // skip
                    }

                    currentPosition = hit.point + currentDirection * skinWidth;
                    reflections++;

                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(reflections, currentPosition);

                    // ToggleActive();
                    Destroy(hit.collider.gameObject);  // Destroy the hit target
                    Instantiate(crystalPrefab, hit.point, Quaternion.identity);
                    break;
                }

                Mirror hitMirror = hit.collider.GetComponent<Mirror>();
                if (hitMirror != null)
                {
                    currentDirection = Vector2.Reflect(currentDirection, hit.normal);
                    // Changes the position to the direction it reflects
                    currentPosition = hit.point + currentDirection * skinWidth;
                    reflections++;

                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(reflections, currentPosition);
                }
                else
                {
                    currentPosition = hit.point + currentDirection * skinWidth;
                    reflections++;
                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(reflections, currentPosition);

                    break;
                }
            }
            else
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(reflections + 1, currentPosition + currentDirection * _maxDistance);
                break;
            }
        }

        foreach (var crystal in crystalsHitLastFrame)
        {
            if (!crystalsHitThisFrame.Contains(crystal))
            {
                crystal.Deactivate();
            }
        }

        var temp = crystalsHitLastFrame;
        crystalsHitLastFrame = crystalsHitThisFrame;
        crystalsHitThisFrame = temp;

    }
}
