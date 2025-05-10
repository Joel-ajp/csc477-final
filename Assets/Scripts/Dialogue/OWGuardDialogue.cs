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

public class OWGuardDialogue : MonoBehaviour
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
        //If any key pressed
        if (Input.anyKeyDown)
        {
            //Select dialogue option
            if(textComponent.text == currentDialogue.dialogueLines[index].dialogue)
            {
                //option 1 (button 1 or e)
                if(index == 0 && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.E)))
                {
                    index = 1;
                    NextLine();
                }
                //option 2 (button 2 or r)
                else if(index == 0 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.R)))
                {
                    index = 2;
                    NextLine();
                }
                //close dialogue
                else if(index > 0)
                {
                    gameObject.SetActive(false);
                }
            }
            //fast forward text scroll
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
        if(index < currentDialogue.dialogueLines.Length)
        {
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
