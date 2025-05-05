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
                    SpriteRenderer end = hit.collider.GetComponent<SpriteRenderer>();
                    end.color = Color.green;

                    break;
                }

                currentDirection = Vector2.Reflect(currentDirection, hit.normal);
                currentPosition = hit.point + currentDirection * skinWidth;
                reflections++;

                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(reflections, currentPosition);
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
