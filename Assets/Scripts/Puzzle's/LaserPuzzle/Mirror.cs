using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : EInteractable
{

    private Vector3 rotationAxis = Vector3.back;
    public float rotationAngle = 15f;

    protected override void DoInteract()
    {
        transform.Rotate(rotationAxis, rotationAngle);
    }

}
