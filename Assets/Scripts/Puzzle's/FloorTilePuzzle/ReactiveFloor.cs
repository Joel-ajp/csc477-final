using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class ReactiveFloor : MonoBehaviour
{
    public TileBase DeactivatedTile;
    public TileBase ActivatedTile;
    public bool listeningPuzzle;
    public List<Vector3Int> requiredTilePositions;
    private HashSet<Vector3Int> _currentActivatedTiles = new HashSet<Vector3Int>();

    // Success State
    public Vector3Int successSpot;
    public TileBase successTile;

    private bool _satisfide;

    private Rigidbody2D _parentRB;
    public Tilemap _currentTilemapFloor; // What tilemap its listening too.

    void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        if (_currentTilemapFloor == null) { return; } // When swap just ignore
        UpdateSurfaceCheck();
    }

    public void UpdateSurfaceCheck()
    {
        Vector2 playerPosition = (Vector2)_parentRB.transform.position; // Player location on grid
        Vector3Int cellPos = _currentTilemapFloor.WorldToCell(playerPosition);
        TileBase tile = _currentTilemapFloor.GetTile(cellPos); // what tile they are on

        if (tile == DeactivatedTile && tile != null)
        {
            _currentTilemapFloor.SetTile(_currentTilemapFloor.WorldToCell(playerPosition), ActivatedTile);
            _currentActivatedTiles.Add(cellPos);

            if (!_satisfide && listeningPuzzle && AllRequiredTilesActivated())
            {
                _satisfide = true;
                _currentTilemapFloor.SetTile(successSpot, successTile);
            }


            StartCoroutine(FloorCooldown(cellPos));
        }
    }

    public void SetCurrentTilemap(Tilemap newTilemap)
    {
        _currentTilemapFloor = newTilemap;
        _currentActivatedTiles.Clear();
        _satisfide = false;
    }

    IEnumerator FloorCooldown(Vector3Int cell)
    {
        yield return new WaitForSeconds(5.0f);
        _currentTilemapFloor.SetTile(cell, DeactivatedTile);
        // Debug.Log("Swapped Back");
    }

    private bool AllRequiredTilesActivated()
    {
        foreach (var required in requiredTilePositions)
        {
            if (!_currentActivatedTiles.Contains(required))
                return false;
        }
        return true;
    }
}

