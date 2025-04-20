using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTag : MonoBehaviour
{
    // Simplest way I can think to do this without messing with the grids.
    // Realistically we should definetly NOT keep it this way and should empliment it with assigning it to a grid type
    // However im tired and this is easy.
    public GroundSurfaceState groundSurfaceState;

    void Start()
    {
        // When it loads get what collider im in
        Collider2D selfCollider = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && selfCollider != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && selfCollider.IsTouching(playerCollider))
            {
                updateSurface(playerCollider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        updateSurface(collision);
    }

    void updateSurface(Collider2D collision)
    {
        // Get the players sound manager for walkingSurface
        WalkingSurface walkingSurface = collision.GetComponentInChildren<WalkingSurface>();

        if (walkingSurface != null)
        {
            walkingSurface.SetSurface(groundSurfaceState);
        }
    }
}
