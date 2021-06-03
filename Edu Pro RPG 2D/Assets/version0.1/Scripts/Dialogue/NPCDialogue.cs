using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;

[RequireComponent(typeof(CircleCollider2D))]
public class NPCDialogue : MonoBehaviour
{
    public string npcName /*, npcDialogue*/;
    public string[] npcDialogueLines;
    public Sprite npcSprite;

    private DialogueManager dialogueManager;
    private bool playerInTheZone;


    public bool hasQuest;
    public int questId;
    public QuestManager questManager;
    public GameObject questStart;

    public TextMeshProUGUI npcDialogoName;


    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerController =  FindObjectOfType<PlayerController>();
        //questStart.SetActive(false);
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
                    finalDialogue[i++]=  line ;
                    /*
                    yield return new WaitForSeconds(0.5f);
                
                }
                */

            }

            if (npcSprite != null && !hasQuest)
            {
                dialogueManager.ShowDialogue(finalDialogue, npcSprite);
            }
            else if (hasQuest)
            {
                Debug.Log("Tengo una mison para ti");
                Quest theQuest = questManager.QuestWithID(questId);
                if (theQuest != null)
                {
                    Debug.Log("La mision existe!");
                    //questStart.SetActive(true);
                    string questLine = theQuest.startText;
                   // dialogueManager.ShowDialogue( questLine, npcSprite);
                
                }
            
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

        if (collision.gameObject.tag.Equals("Player"))
        {
            playerInTheZone = true;
            npcDialogoName.text = npcName;
            //playerController.playerCapCol.isTrigger = true;
            Debug.Log("El jugador esta en la zona");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            playerInTheZone = false;
            //playerController.playerCapCol.isTrigger = false;

            Debug.Log("El jugador ya no esta en la zona");
        }
    }
}
