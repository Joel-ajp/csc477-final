using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkingSurface : MonoBehaviour
{
    // Im getting the parent to see if its moving
    private Rigidbody2D _parentRB;

    // Only set to public currently for testing purposes, intended to be private
    private SoundType _currentSurfaceSound;
    private float _pitchSpeed;
    private bool _walking;

    private Tilemap _currentTilemapFloor;
    private GameObject _lastFloorObject; // Need to get the object for the related tag

    [SerializeField]
    private List<FloorTileData> tileData;
    private Dictionary<TileBase, GroundSurfaceState> audioData;

    private void Awake()
    {
        // Fills the dictionary
        PopulateTileSoundDictionary();
    }


    // Start is called before the first frame update
    void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody2D>();
        SetSurface(GroundSurfaceState.WOOD); // default
    }

    // Update is called once per frame
    void Update()
    {
        // Look for what the active tilemap is
        GameObject currentFloorObj = GameObject.FindGameObjectWithTag("FloorTilemap");
        // Checks if it exists and is active, aswell as making sure its not needlessly updating
        if (currentFloorObj != null && currentFloorObj.activeInHierarchy && currentFloorObj != _lastFloorObject)
        {
            Tilemap newTilemap = currentFloorObj.GetComponent<Tilemap>();
            if (newTilemap != null)
            {
                _currentTilemapFloor = newTilemap;
                _lastFloorObject = currentFloorObj;
                // Debug.Log($"Active: {currentFloorObj.name}");
            }
        }

        // This is a roundabout way of doing it, didnt want to mess with movement script yet, this can be improved later
        if (_parentRB.velocity.magnitude > 0.05f && !_walking && _currentTilemapFloor != null)
        {
            UpdateSurfaceCheck();
        }
    }

    // This specifically waits the length of the clip so it doesnt cut off
    IEnumerator PlayWalkSound()
    {
        _walking = true;
        AudioClip clip = SoundManager.Instance.GetClip(_currentSurfaceSound);
        if (clip != null)
        {
            // Makes sure that the wait time is based off the proper speed
            float clipLengthAdjust = clip.length / _pitchSpeed;

            SoundManager.Instance.Play(_currentSurfaceSound, _pitchSpeed);
            yield return new WaitForSeconds(clipLengthAdjust);
        }
        _walking = false;
    }

    public void UpdateSurfaceCheck()
    {
        Vector2 playerPosition = (Vector2)_parentRB.transform.position; // Player location on grid
        TileBase tile = _currentTilemapFloor.GetTile(_currentTilemapFloor.WorldToCell(playerPosition)); // what tile they are on
        if (tile != null)
        {
            if (audioData.ContainsKey(tile))
            {
                // Set the surface sound and update the surface state
                SetSurface(audioData[tile]); // Uses the dictionary made above
                StartCoroutine(PlayWalkSound());
            }
            else
            {
                StartCoroutine(PlayWalkSound());
            }
        }
        else
        {

            StartCoroutine(PlayWalkSound());
        }
    }

    public void PopulateTileSoundDictionary()
    {
        // load all the available tile sounds from audio folder stuff
        tileData = new List<FloorTileData>(Resources.LoadAll<FloorTileData>("Movement/FloorTileAudioAssign"));
        audioData = new Dictionary<TileBase, GroundSurfaceState>();

        foreach (var tileGroup in tileData)
        {
            foreach (var tile in tileGroup.surfaceTiles)
            {
                if (tile != null && !audioData.ContainsKey(tile))
                {
                    audioData.Add(tile, tileGroup.surfaceType);
                }
            }
        }

    }

    public void SetSurface(GroundSurfaceState state)
    {
        // Pitch is currently being used to match speed with animation
        switch (state)
        {
            case GroundSurfaceState.CONCRETE:
                _currentSurfaceSound = SoundType.WALK_CONCRETE;
                _pitchSpeed = 1.75f;
                break;
            case GroundSurfaceState.WOOD:
                _currentSurfaceSound = SoundType.WALK_WOOD;
                _pitchSpeed = 1.25f;
                break;
            case GroundSurfaceState.METAL:
                _currentSurfaceSound = SoundType.WALK_METAL;
                _pitchSpeed = 1.3f;
                break;
            case GroundSurfaceState.SAND:
                _currentSurfaceSound = SoundType.WALK_SAND;
                _pitchSpeed = 1.4f;
                break;
        }
    }


}
