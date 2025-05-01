using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class ReactiveFloor : MonoBehaviour
{
    public TileBase DeactivatedTile;
    public TileBase ActivatedTile;
    private bool _satisfide;

    private Rigidbody2D _parentRB;
    public Tilemap _currentTilemapFloor; // { private get; set; } use this once switching is set up

    void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateSurfaceCheck();
    }

    public void UpdateSurfaceCheck()
    {
        Vector2 playerPosition = (Vector2)_parentRB.transform.position; // Player location on grid
        TileBase tile = _currentTilemapFloor.GetTile(_currentTilemapFloor.WorldToCell(playerPosition)); // what tile they are on

        if (tile == DeactivatedTile && tile != null)
        {
            _currentTilemapFloor.SetTile(_currentTilemapFloor.WorldToCell(playerPosition), ActivatedTile);
            Debug.Log("Swapped Tile");

            StartCoroutine(FloorCooldown(playerPosition));
        }
    }


    IEnumerator FloorCooldown(Vector2 playerPosition)
    {
        yield return new WaitForSeconds(5.0f);
        _currentTilemapFloor.SetTile(_currentTilemapFloor.WorldToCell(playerPosition), DeactivatedTile);
        Debug.Log("Swapped Back");
    }
}

