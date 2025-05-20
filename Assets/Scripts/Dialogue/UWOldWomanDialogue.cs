using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class UWOldWomanDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private DialogueObject _currentDialogue;
    public float textSpeed; //scroll speed
    private int index;  //line of dialogue
    private PlayerMovement playerMovement;
    private EnvironmentManager sceneSwap;

    //Start dialogue
    void OnEnable()
    {
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
        //disable movement
        playerMovement._movementEnabled = false;
        sceneSwap.swapAllowed = false;
    }

    private void OnDisable()
    {
        //reenable movement
        playerMovement._movementEnabled = true;
        sceneSwap.swapAllowed = true;
    }

    void Awake() // At the start load the required Dialogue
    {
        _currentDialogue = Resources.Load<DialogueObject>("Dialogue/UWOldWomanDialogue");
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        sceneSwap = GameObject.Find("GameManager").GetComponent<EnvironmentManager>();
    }

    private void Update()
    {
        //If any key pressed
        if (Input.anyKeyDown)
        {
            //exit state is escape pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopAllCoroutines();
                gameObject.SetActive(false);
            }
            //Select dialogue option
            else if (textComponent.text == _currentDialogue.dialogueLines[index].dialogue)
            {
                //option 1 (button 1 or e)
                if ((index == 0) && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.E)))
                {
                    index = 1;
                    NextLine();
                }
                //option 2 (button 2 or r)
                else if ((index == 0 || index == 2) && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.R)))
                {
                    index = 3;
                    NextLine();
                }
                //continue option text
                else if (index == 1 || index >= 3 && index <= 8)
                {
                    index++;
                    NextLine();
                }
                //exit state
                else if (index == 2 || index > 8)
                {
                    gameObject.SetActive(false);
                }
            }
            //fast forward text scroll
            else
            {
                StopAllCoroutines();
                textComponent.text = _currentDialogue.dialogueLines[index].dialogue;
            }
        }
    }

    IEnumerator TypeLine()
    {
        //reset text box
        textComponent.text = string.Empty;

        //type 1 letter at a time
        foreach (char c in _currentDialogue.dialogueLines[index].dialogue.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < _currentDialogue.dialogueLines.Length)
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
