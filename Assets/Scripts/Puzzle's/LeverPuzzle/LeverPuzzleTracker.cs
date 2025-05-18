using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzleTracker : MonoBehaviour
{
    public List<LeverScript> levers;
    public List<DoorPuzzle> Doors;
    private bool _endDoorOpen;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        StartCoroutine(CheckCompletion());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CheckCompletion()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Check every half second
            // Debug.Log("Door Is Checked");
            if (CheckLeverState() && !_endDoorOpen)
            {
                _endDoorOpen = true;
                OpenDoors();
            }
            else if (!CheckLeverState() && _endDoorOpen)
            {
                _endDoorOpen = false;
                CloseDoors();
            }
        }
    }

    bool CheckLeverState()
    {
        foreach (var lever in levers)
        {
            if (lever.isActive == false)
            {
                return false;
            }
        }
        return true;
    }

    void OpenDoors()
    {
        foreach (var door in Doors)
        {
            door.SetDoorState(DoorPuzzle.DoorState.OPEN);
        }
    }

    void CloseDoors()
    {
        foreach (var door in Doors)
        {
            door.SetDoorState(DoorPuzzle.DoorState.CLOSED);
        }
    }
}
