using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu] // makes instance in editor
public class FloorTileData : ScriptableObject
{
    public GroundSurfaceState surfaceType; // Set the ground here
    public TileBase[] surfaceTiles; // Add the tiles you want associated with this sound
}
