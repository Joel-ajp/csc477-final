using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : EInteractable // Inherits EInteractable and changes what happens when interaction occours.
{
    public bool isActive = false;
    public Transform pivot;

    protected override void DoInteract() // Changes DoInteract in EInteractable to this, can be used with same logic
    {
        isActive = !isActive;
        SoundManager.Instance.Play(SoundType.CLICK);

        if (isActive)
        {
            pivot.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            pivot.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}