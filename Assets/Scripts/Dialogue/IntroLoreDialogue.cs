using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroLoreDialogue : MonoBehaviour
{
     public TextMeshProUGUI textComponent;
    private DialogueObject _currentDialogue;
    public float textSpeed;
    private int index;
    private PlayerMovement playerMovement;
    private EnvironmentManager sceneSwap;

    void Awake() // At the start load the required Dialogue
    {
        _currentDialogue = Resources.Load<DialogueObject>("Dialogue/IntroLoreDialogue");
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        sceneSwap = GameObject.Find("GameManager").GetComponent<EnvironmentManager>();
    }

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

    private void Update()
    {
        //If any key pressed
        if (Input.anyKeyDown)
        {
            // go through dialogue
            if (textComponent.text == _currentDialogue.dialogueLines[index].dialogue)
            {
                if (index < 5 && Input.GetKeyDown(KeyCode.E))
                {
                    index++;
                    NextLine();
                }
                //close dialogue
                else if (index >= 5)
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
