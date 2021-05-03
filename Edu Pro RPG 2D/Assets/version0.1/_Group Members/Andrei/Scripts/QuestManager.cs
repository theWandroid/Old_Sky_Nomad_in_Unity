using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    //va a ser una lista de quests
    public List<Quest> quests;
    private DialogueManager dialogueManager;
    public QuestItem itemCollected;
    public QuestEnemy enemyKilled;

   

    // Start is called before the first frame update
    void Start()
    {
        //para ser capaces de escribir el texto de la misión en la zona del dialogo
        dialogueManager = FindObjectOfType<DialogueManager>();
        //recorrera todos los hijos del QuestManager, que serán los Quest
        foreach (Transform t in transform)
        {
            quests.Add(t.gameObject.GetComponent<Quest>());
        }
        Debug.Log("Hay {0} quests." + quests.Count);
        
    }

    public void ShowQuestText(string questText)
    {
        dialogueManager.ShowDialogue(new string[] { questText });

        Debug.Log("Estoy hablando, y me tengo que parar");
        return;
    }

    public Quest QuestWithID(int questID)
    {
        Quest q = null;
        foreach(Quest temp in quests)
        {//si esa quest temporal concuerda con la mison actual
            if(temp.questID == questID)
            {
                q = temp;
            }
        }
        return q;
    }
}
