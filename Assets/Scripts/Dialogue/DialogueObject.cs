using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "DialogueDialogueObject")]
public class DialogueObject : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}