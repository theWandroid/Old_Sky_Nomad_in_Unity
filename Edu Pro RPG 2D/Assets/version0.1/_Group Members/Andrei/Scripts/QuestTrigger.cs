using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    private QuestManager questManager;
    public int questID;
    public bool startPoint, endPoint;
    private bool playerInZone;
    public bool automaticCatch;

    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerInZone = true;
            Debug.Log("El jugador ha entrado");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerInZone = false;
        }
    }

    private void Update()
    {
        if (playerInZone)
        {
            Debug.Log("Jugador en la zona");
            if(automaticCatch || !automaticCatch && Input.GetMouseButtonDown(0)){
                Quest q = questManager.QuestWithID(questID);
                if (q == null)
                {
                    Debug.LogErrorFormat("La misión con ID {0} no existe", questID);
                    return;
                }
                Debug.Log("Tengo misión");
                //si llego aquí la miisón existe
                //no he completado la mision actual
                // si quitamos esta linea la mision es repetible
                if (!q.questCompleted)
                {
                    Debug.Log("No completada");
                    //estoy en la zona que empieza la misión
                    if (startPoint)
                    {
                        Debug.Log("Punto de inicio");
                        //si no estava activada la activamos entonces
                        if (!q.gameObject.activeInHierarchy)
                        {
                            Debug.Log("Procedemos a activarla");
                            //arranca la misión
                            q.gameObject.SetActive(true);
                            q.StartQuest();
                        }
                    }
                    //estoy en la zona que acaba la misión
                    if (endPoint)
                    {//si la mision estava activa en la jerarquia
                        Debug.Log("Punto de fin");
                        if (q.gameObject.activeInHierarchy)
                        {//la completamos
                            Debug.Log("Completamos misión");
                            q.CompleteQuest();
                        }
                    }

                }
                //si no he completado la mision actual
                /*
                if (!questManager.quests[questID].questCompleted)
                {

                }*/
            }
        }
    }
}
