﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CircleCollider2D))]
public class NPCDialogue : MonoBehaviour
{
    public string npcName /*, npcDialogue*/;
    public string[] npcDialogueLines;
    public Sprite npcSprite;

    private DialogueManager dialogueManager;
    private bool playerInTheZone;
    public bool hasQuest;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInTheZone && CrossPlatformInputManager.GetButtonDown("Action")){
            /*
            string finalDialogue;
            if (npcName != null)
            {
                finalDialogue = npcName + "\n" + npcDialogue;
            }
            else
            {
                finalDialogue = npcDialogue;
            }*/
            //haciendo lo anterior con el operador ternario
            //string finalDialogue = (npcName !=null ? npcName + "\n" : "") + npcDialogue;
            //lo inicializamos vacio



            Debug.Log("Voy a hablar");
            string[] finalDialogue = new string[npcDialogueLines.Length];
            //para cada linea de dialogo, recorro todas las lineas de dialogo
            int i = 0;

            foreach (string line in npcDialogueLines)
            {
                /*
                foreach (char character in line)
                {
                    finalDialogue[i++] = (npcName != null ? npcName + " \n" : "") + character + line;
                    */
                    finalDialogue[i++]= (npcName != null ? npcName + " \n" : "") +  line ;
                    /*
                    yield return new WaitForSeconds(0.5f);
                
                }
                */

            }

            if (npcSprite != null)
            {
                dialogueManager.ShowDialogue(finalDialogue, npcSprite);
            }
            else 
            { 
            dialogueManager.ShowDialogue(finalDialogue);
            }

            //Si el NPCMovement del padre no es nulo, es decir que si el padre tiene el componente NPCMovement
            if (gameObject.GetComponentInParent<NPCMovement>()!= null)
            {//entonces modificamos la variable del Componente padre y idicamos que esta hablando
                gameObject.GetComponentInParent<NPCMovement>().isTalking = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController;

        if (collision.gameObject.tag.Equals("Player"))
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            Debug.Log(playerController.lastMovement);
            playerInTheZone = true;
            Debug.Log("El jugador esta en la zona");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            playerInTheZone = false;
            Debug.Log("El jugador ya no esta en la zona");
        }
    }
}
