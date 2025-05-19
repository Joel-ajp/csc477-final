using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UWShopDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private DialogueObject _currentDialogue;
    public float textSpeed; //scroll speed
    private int index;  //line of dialogue
    private bool seenBefore;    //if player has already talked to shopkeeper
    public Shopkeeper keeperui;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        seenBefore = false;
    }

    //Start dialogue
    void OnEnable()
    {
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
        //disable movement
        playerMovement._movementEnabled = false;
    }

    private void OnDisable()
    {
        //reenable movement
        playerMovement._movementEnabled = true;
    }

    void Awake() // At the start load the required Dialogue
    {
        _currentDialogue = Resources.Load<DialogueObject>("Dialogue/UWShopDialogue");
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
                if ((index == 0 || index == 6) && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.E)))
                {
                    index = 1;
                    NextLine();
                }
                //option 2 (button 2 or r)
                else if ((index == 0 || index == 6) && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.R)))
                {
                    index = seenBefore ? 7 : 2;
                    NextLine();
                }
                //transition to the shop screen
                else if (index == 1)
                {
                    //TRANSFER TO SHOP
                    keeperui.ToggleShop();
                    gameObject.SetActive(false);
                }
                //continue option 2 text
                else if (index >= 2 && index <= 5)
                {
                    index++;
                    seenBefore = true;
                    NextLine();
                }
                //deja vu addition if player already chosen this option
                else if (index == 7)
                {
                    index = 3;
                    NextLine();
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
