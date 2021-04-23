using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class QuestItem : MonoBehaviour
{
    public int questID;
    private QuestManager questManager;
    private ItemsManager itemManager;
    public string itemName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player")) {
            questManager = FindObjectOfType<QuestManager>();
            itemManager = FindObjectOfType<ItemsManager>();
            Quest theQuest = questManager.QuestWithID(questID);
            if (theQuest == null)
            {
                Debug.LogErrorFormat("La misión con id {0} no existe", questID);
            }
            if (theQuest.gameObject.activeInHierarchy && !theQuest.questCompleted)
            {
                questManager.itemCollected = this;
                //añadimos el item recogido
                itemManager.AddQuestItem(this.gameObject);
                //desactivamos el item actual
                gameObject.SetActive(false);
            }
        }
    }
}
