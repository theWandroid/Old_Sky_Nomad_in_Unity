using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public Image avatarImage;
    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentDialogueLine;

    private PlayerController playerController;

    private GameObject joystick;
    private GameObject handle;
    private GameObject actionButton;

    private GameObject continueButton;


    private void Start()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);
        //localizo al player por su variable
        playerController = FindObjectOfType<PlayerController>();

        joystick = GameObject.Find("Joystick");
        handle = GameObject.Find("Handle");
        actionButton = GameObject.Find("Action Button");
        continueButton = GameObject.Find("Continue Button");
        continueButton.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {

        if (CrossPlatformInputManager.GetButtonDown("Continue"))
        {
            Debug.Log("Se ha detectado la pulsación");

        currentDialogueLine++;
        /*
        dialogueActive = false;
        avatarImage.enabled = false;
        dialogueBox.SetActive(false);
        */
        //si la linea actual es mayor o igual que las lineas de dialogo
        if (currentDialogueLine >= dialogueLines.Length)
        {
            currentDialogueLine = 0;
            dialogueActive = false;
            avatarImage.enabled = false;
            dialogueBox.SetActive(false);
            playerController.isTalking = false;
            joystick.gameObject.GetComponent<Image>().enabled = true;
            handle.gameObject.GetComponent<Image>().enabled = true;
            actionButton.GetComponent<Image>().enabled = true;
            continueButton.SetActive(false);


        }
        else
        {
            dialogueText.text = dialogueLines[currentDialogueLine];
        }}
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
        joystick.GetComponent<Image>().enabled =false;
        handle.GetComponent<Image>().enabled = false;
        actionButton.GetComponent<Image>().enabled = false;
        continueButton.SetActive(true);
    }

    public void ShowDialogue(string [] lines, Sprite sprite)
    {
        ShowDialogue(lines);
        avatarImage.enabled = true;
        avatarImage.sprite = sprite;
    }
    /*
    private void DesactiveControls()
    {
        joystick.SetActive(false);
        handle.SetActive(false);
        actionButton.SetActive(false);
    
    }

    private void ActiveControls()
    {
        joystick.SetActive(true);
        handle.SetActive(true);
        actionButton.SetActive(true);
    }
    */
}
