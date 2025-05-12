using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class ReactiveFloor : MonoBehaviour
{
    [Header("Tile References")]
    public List<TileBase> DeactivatedTiles;
    public List<TileBase> ActivatedTiles;
    public bool swapSound;
    public Tilemap _currentTilemapFloor; // What tilemap its listening too.
    public bool swapBack;
    public float vanishSpeedSeconds = 2f;

    [Header("Puzzle Stuff")]
    public bool listeningPuzzle;
    public bool attachToPlayer = true;
    public List<Vector3Int> requiredTilePositions;
    public Transform crystalSpawnPoint;
    public GameObject crystalReward;
    // Success State
    // public Vector3Int successSpot;
    // public TileBase successTile;

    private bool _satisfide;
    private HashSet<Vector3Int> _currentActivatedTiles = new HashSet<Vector3Int>();
    private Rigidbody2D _parentRB;
    private bool cancelCooldowns = false;

    void Start()
    {
        if (attachToPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                transform.SetParent(player.transform, false);
                _parentRB = player.GetComponent<Rigidbody2D>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("player null");
            }
        }
        else
        {
            _parentRB = GetComponentInParent<Rigidbody2D>();
        }

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
            if (swapSound) SoundManager.Instance.Play(SoundType.POP);

            _currentTilemapFloor.SetTile(_currentTilemapFloor.WorldToCell(playerPosition), randomActiveTile);
            _currentActivatedTiles.Add(cellPos);

            if (!_satisfide && listeningPuzzle && AllRequiredTilesActivated())
            {
                cancelCooldowns = true; // Stops the corutines once all states are satisfied
                _satisfide = true;
                SoundManager.Instance.Play(SoundType.SUCCESS);
                CreateCrystal();

            }
            if (swapBack && !_satisfide) // Prevents it from starting new if coruntine exists
            {
                StartCoroutine(FloorCooldown(cellPos, originalTile));
            }
        }
    }

    public void CreateCrystal()
    {
        if (crystalReward == null || crystalSpawnPoint == null)
        {
            Debug.LogWarning("Missing crystal spawn stuff");
            return;
        }

        GameObject spawnedCrystal = Instantiate(crystalReward, crystalSpawnPoint.position, Quaternion.identity);
        spawnedCrystal.transform.SetParent(crystalSpawnPoint.transform, true); // This is to set it in other worlds
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
        if (cancelCooldowns) yield break;
        _currentActivatedTiles.Remove(cell); // Remove from the listened list
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

