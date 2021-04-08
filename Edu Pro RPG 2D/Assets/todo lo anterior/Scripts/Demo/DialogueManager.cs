using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public Image avatarImage;
    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentDialogueLine;

    private PlayerController playerController;

    private void Start()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);
        //localizo al player por su variable
        playerController = FindObjectOfType<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            //cada vez que pulsamos espacio se pasa a la siguiente linea de dialogo
            currentDialogueLine++;
            /*
            dialogueActive = false;
            avatarImage.enabled = false;
            dialogueBox.SetActive(false);
            */
        
            //si la linea actual es mayor o igual que las lineas de dialogo
            if (currentDialogueLine>= dialogueLines.Length)
            {
                currentDialogueLine = 0;
                dialogueActive = false;
                avatarImage.enabled = false;
                dialogueBox.SetActive(false);
                playerController.isTalking = false;
            }
            else
            {
                dialogueText.text = dialogueLines[currentDialogueLine];
            }
        }

    }

    public void ShowDialogue(string [] lines)
    {
        currentDialogueLine = 0;
        dialogueLines = lines;
        dialogueActive = true;
        dialogueBox.SetActive(true);
        //dialogueText.text = text;
        dialogueText.text = dialogueLines[currentDialogueLine];
        playerController.isTalking = true;
    }

    public void ShowDialogue(string [] lines, Sprite sprite)
    {
        ShowDialogue(lines);
        avatarImage.enabled = true;
        avatarImage.sprite = sprite;
    }
}
