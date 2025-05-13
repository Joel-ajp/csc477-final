using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerDialogue : EInteractable
{
    public GameObject dialogueCanvas;

    //Trigger dialogue when used on npc (call prefab)
    protected override void DoInteract()
    {
        print("activated");
        dialogueCanvas.SetActive(true);
    }
}