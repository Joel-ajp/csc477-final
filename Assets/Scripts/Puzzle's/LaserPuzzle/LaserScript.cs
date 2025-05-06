using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private int _maxReflections = 5;
    private float _maxDistance = 100f;
    private LineRenderer _lineRenderer;
    public bool isActive;
    public Transform Barrel;
    public LayerMask foreground;
    public GameObject crystalPrefab;


    void Start()
    {
        _lineRenderer = Barrel.GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isActive)
        {
            DrawLaser(Barrel.position, Barrel.right);
        }
    }

    void ToggleActive()
    {
        isActive = !isActive;
    }

    void DrawLaser(Vector2 startPos, Vector2 direction)
    {
        Vector2 currentPosition = startPos;
        Vector2 currentDirection = direction;
        const float skinWidth = 0.01f;

        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, currentPosition);

        int reflections = 0;

        while (reflections < _maxReflections)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, _maxDistance, foreground);

            if (hit.collider != null)
            {
                if (hit.collider.name == "LaserHitPoint")
                {

                    // Draws to the hit point, doesnt reflect
                    // currentPosition = hit.point + currentDirection * skinWidth;
                    // reflections++;
                    // _lineRenderer.positionCount++;
                    // _lineRenderer.SetPosition(reflections, currentPosition);
                    _lineRenderer.enabled.Equals(false);

                    // Success Result, put end logic here
                    isActive = false;
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
    }
}
