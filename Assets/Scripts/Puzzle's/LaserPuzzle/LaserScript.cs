using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public int maxReflections = 5;
    public float maxDistance = 100f;
    private LineRenderer _lineRenderer;
    public LayerMask reflectionMask;
    public bool isActive;


    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isActive)
        {
            DrawLaser(transform.position, transform.right);
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

        while (reflections < maxReflections)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, maxDistance, reflectionMask);

            if (hit.collider != null)
            {
                if (hit.collider.name == "laserEndPoint")
                {
                    Debug.Log("Laser hit end");
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
                _lineRenderer.SetPosition(reflections + 1, currentPosition + currentDirection * maxDistance);
                break;
            }
        }
    }
}
