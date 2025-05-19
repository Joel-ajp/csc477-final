using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDoor : MonoBehaviour
{
    public List<CrystalTargets> crystalTargets;
    public GameObject LaserEmitter;
    private bool _success = false;
    public Gate door;

    // Check if all crystals are active
    public bool AllActive()
    {
        foreach (var crystal in crystalTargets)
        {
            if (crystal == null || !crystal.isActive)
                return false;
        }

        return true;
    }

    void Update()
    {
        if (!_success && AllActive())
        {
            _success = true;
            LaserScript laserScript = LaserEmitter.GetComponent<LaserScript>();
            laserScript.ToggleActive();
            door.ToggleGate();
            Debug.Log("YIPEE");

        }
    }

    void openDoor()
    {

    }
}
