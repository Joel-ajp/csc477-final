using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string dialogue;

}

[CreateAssetMenu(menuName = "DialogueDialogueObject")]
public class DialogueObject : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}

public class GuardDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public DialogueObject currentDialogue;
    public float textSpeed;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        index = 0;
        //Start the dialogue
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //fast forward text scroll on click
            if(textComponent.text == currentDialogue.dialogueLines[index].dialogue)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = currentDialogue.dialogueLines[index].dialogue;
            }
        }
    }

    IEnumerator TypeLine()
    {
        //reset text box
        textComponent.text = string.Empty;

        //type 1 letter at a time
        foreach (char c in currentDialogue.dialogueLines[index].dialogue.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < currentDialogue.dialogueLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
