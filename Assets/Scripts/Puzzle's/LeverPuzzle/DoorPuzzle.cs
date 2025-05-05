using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorPuzzle : MonoBehaviour
{
    public Transform pivot;
    public bool rightPivot;

    public enum DoorState
    {
        CLOSED,
        OPENING,
        CLOSING,
        OPEN
    }

    public DoorState _currentState { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _currentState = DoorState.CLOSED;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void SetDoorState(DoorState state)
    {

        _currentState = state;
        // Debug.Log("Switching to  " + state.ToString());
        switch (state)
        {
            case DoorState.CLOSED:
                pivot.localRotation = Quaternion.Euler(0f, 0f, 0);
                break;
            case DoorState.OPEN:
                if (rightPivot)
                {
                    pivot.localRotation = Quaternion.Euler(0f, 0f, 90f);
                }
                else
                {
                    pivot.localRotation = Quaternion.Euler(0f, 0f, -90f);
                }
                break;
        }
    }
}
