using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNewPlace : MonoBehaviour
{
    //Nombre de la escena a la que voy a teletransportar al jugador
    public string newPlaceName;
    public string uuid;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().nextUuid = uuid;
            SceneManager.LoadScene(newPlaceName);
        }
    }

}
