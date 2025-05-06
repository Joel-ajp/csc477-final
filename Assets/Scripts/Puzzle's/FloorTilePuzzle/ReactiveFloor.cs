using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class ReactiveFloor : MonoBehaviour
{
    [Header("Tile References")]
    public List<TileBase> DeactivatedTiles;
    public List<TileBase> ActivatedTiles;
    public Tilemap _currentTilemapFloor; // What tilemap its listening too.
    public float vanishSpeedSeconds = 2f;

    [Header("Puzzle Stuff")]
    public bool listeningPuzzle;
    public List<Vector3Int> requiredTilePositions;
    // Success State
    public Vector3Int successSpot;
    public TileBase successTile;

    private bool _satisfide;
    private HashSet<Vector3Int> _currentActivatedTiles = new HashSet<Vector3Int>();
    private Rigidbody2D _parentRB;

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
        TileBase originalTile = tile;
        if (IsDeactivatedTile(tile) && tile != null)
        {
            TileBase randomActiveTile = GetRandomActivatedTile();
            if (randomActiveTile == null) return;

            _currentTilemapFloor.SetTile(_currentTilemapFloor.WorldToCell(playerPosition), randomActiveTile);
            _currentActivatedTiles.Add(cellPos);

            if (!_satisfide && listeningPuzzle && AllRequiredTilesActivated())
            {
                _satisfide = true;
                _currentTilemapFloor.SetTile(successSpot, successTile);
            }

            StartCoroutine(FloorCooldown(cellPos, originalTile));
        }
    }

    public void SetCurrentTilemap(Tilemap newTilemap)
    {
        _currentTilemapFloor = newTilemap;
        _currentActivatedTiles.Clear();
        _satisfide = false;
    }

    IEnumerator FloorCooldown(Vector3Int cell, TileBase originalTile)
    {
        yield return new WaitForSeconds(vanishSpeedSeconds);
        _currentTilemapFloor.SetTile(cell, originalTile);
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

    private bool IsDeactivatedTile(TileBase tile)
    {
        return DeactivatedTiles.Contains(tile);
    }

    private TileBase GetRandomActivatedTile()
    {
        if (ActivatedTiles.Count == 0)
        {
            return null;
        }
        return ActivatedTiles[Random.Range(0, ActivatedTiles.Count)];
    }
}

