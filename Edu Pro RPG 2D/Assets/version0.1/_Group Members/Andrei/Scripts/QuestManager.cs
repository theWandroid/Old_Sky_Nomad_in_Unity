using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    //va a ser una lista de quests
    public List<Quest> quests;
    private DialogueManager dialogueManager;
    public QuestItem itemCollected;
    public QuestEnemy enemyKilled;
    public int actualQuest;


    public PlayerController playerController;
    public CameraFollow cameraFollow;
   

    // Start is called before the first frame update
    void Start()
    {
        //para ser capaces de escribir el texto de la misión en la zona del dialogo
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerController = FindObjectOfType<PlayerController>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        //recorrera todos los hijos del QuestManager, que serán los Quest
        foreach (Transform t in transform)
        {
            quests.Add(t.gameObject.GetComponent<Quest>());
        }
        Debug.Log("Hay" +quests.Count+ " quests." );
        
    }

    public void ShowQuestText(string questText, Sprite npcSprite)
    {
        dialogueManager.ShowDialogue(new string[] { questText }, npcSprite);
        Debug.Log(quests[actualQuest]);
        Debug.Log(quests[actualQuest].npcSprite);
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

    public void AcceptQuest()
    {
        Debug.Log("Ha aceptado la misión");
        dialogueManager.ShowDialogue(new string[] { quests[actualQuest].acceptQuestText }, quests[actualQuest].npcSprite);
        quests[actualQuest].questCompleted = true;
        DisableConfirmation();
        StartCoroutine(Utilities.LoadTwoSeconds());
        playerController.SavePlayerPosition();
        cameraFollow.SaveCameraPosition();
        SceneManager.LoadScene(quests[actualQuest].questScene);
    }

    public void DenyQuest()
    {
        Debug.Log("No ha aceptado la misión");
        dialogueManager.ShowDialogue(new string[] { quests[actualQuest].denyQuestText }, quests[actualQuest].npcSprite);
        DisableConfirmation();
    }

    void DisableConfirmation()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager.DesactiveConfirmation();
    }

}
