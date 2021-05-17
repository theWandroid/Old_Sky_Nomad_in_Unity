using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quest : MonoBehaviour
{
    // los identificadores de las quesst
    public int questID;
    public bool questCompleted;
    public bool questStarted;
    private QuestManager questManager;

    public string title;
    public string startText;
    public string completeText;

    public bool needsItem;
    public List<QuestItem> itemsNeeded;

    public bool killsEnemy;
    //lista de posibles enemigos
    public List<QuestEnemy> enemies;
    //cuantos debemos de matar de cada uno
    public List<int> numberOfEnemies;

    public Quest nextQuest;

    public bool confirmacion;

    private UIManager uIManager;

    public string questScene;

    // Start is called before the first frame update

    private void Start()
    {
        
    }

    /*
     private void OnEnable(int level)
     {
         SceneManager.sceneLoaded += OnSceneLoaded;
     }
    */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) { 
        if (needsItem)
        {
            ActivateItems();
        }
        if (killsEnemy)
        {
            ActivateEnemies();
        }
    }

    public void StartQuest()
    {
        //SFXManagerSingleton.SharedInstance.PlaySFX(SFXType.SoundType.M_START);
        uIManager = FindObjectOfType<UIManager>();
        questManager = FindObjectOfType<QuestManager>();
        questManager.ShowQuestText(title + "\n" + startText);

        if (needsItem)
        {
            ActivateItems();
        }

        if (killsEnemy)
        {
            ActivateEnemies();
        }

        if (confirmacion)
        {
            ActivateConfirmacion();
        }

    }

    void ActivateItems()
    {
        //es un array de objectos questItem
        //Object [] qItems = Resources.FindObjectsOfTypeAll(typeof(QuestItem));
        Object[] qItems = Resources.FindObjectsOfTypeAll<QuestItem>();

        foreach (QuestItem qItem in qItems)
        {
            if (qItem.questID == questID)
            {
                qItem.gameObject.SetActive(true);
            }
        }
    }

    void ActivateEnemies()
    {

        Object[] qEnemies = Resources.FindObjectsOfTypeAll<QuestEnemy>();
        
        foreach (QuestEnemy qEnemy in qEnemies)
        {
            if (qEnemy.questID == questID)
            {
                qEnemy.gameObject.SetActive(true);
            }
        }
    }

    public void CompleteQuest()
    {
        //SFXManagerSingleton.SharedInstance.PlaySFX(SFXType.SoundType.M_END);
        questManager = FindObjectOfType<QuestManager>();
        questManager.ShowQuestText(title + "\n" + completeText);
        questCompleted = true;

        if(nextQuest != null)
        {
            //si lo hacemos tal qual, directamente después de terminar una se nos activaria la siguiente, por eso lo haremos con el metodo Invoke
            //ActivateNextQuest();
            Invoke("ActivateNextQuest", 5.0f);
        }
        //si no la desactivamos la mision se podria hacer mucahs veces
        gameObject.SetActive(false);
    }

    void ActivateNextQuest()
    {
        nextQuest.gameObject.SetActive(true);
        nextQuest.StartQuest();
    }

    void ActivateConfirmacion()
    {
        uIManager.ActiveConfirmacion();
    }


    private void Update()
    {
        if(needsItem && questManager.itemCollected != null)
        {
            for(int i =0; i < itemsNeeded.Count; i++)
            {
                if(itemsNeeded[i].itemName == questManager.itemCollected.itemName)
                {
                    Debug.Log("TEnemos un item en el manager");
                    itemsNeeded.RemoveAt(i);
                    Debug.Log("Item encontrado");
                    questManager.itemCollected = null;
                    //hacemos un break, para que el bucle no siga
                    break;
                }
            }
            if(itemsNeeded.Count == 0)
            {
                Debug.Log("Mision ompletada");
                CompleteQuest();
            }
        }

        //si se trat de una mision de matar enemigos y acabamos de matar a uno
        if(killsEnemy && questManager.enemyKilled != null)
        {
            Debug.Log("Tenemos un enemigo recien matado");
            for(int i=0; i < enemies.Count; i++)
            {//lo localizamos por enemyName
                if(enemies[i].enemyName == questManager.enemyKilled.enemyName)
                {
                    //decremento en 1 el numero de enemigos que he de matar
                    numberOfEnemies[i]--;
                    questManager.enemyKilled = null;
                    // en el caso de que me quede con 0
                    if(numberOfEnemies[i] <= 0)
                    {
                        //elimino de la lista el enemigo
                        enemies.RemoveAt(i);
                        numberOfEnemies.RemoveAt(i);
                    }
                    //para que el bucle no siga buscando
                    break;
                }
            }
            //completo la mision en el caso de que me haya quedado sin enemigos restantes
            if(enemies.Count == 0)
            {
                CompleteQuest();
            }
        }
    }


    public void AcceptQuest()
    {
        Debug.Log("Ha aceptado la misión");
        SceneManager.LoadScene(questScene);
    }

    public void DenyQuest()
    {
        Debug.Log("No ha aceptado la misión");
    }

}

