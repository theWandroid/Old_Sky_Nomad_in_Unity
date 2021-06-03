using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public List<GameObject> playerCharacters;

    private CameraFollow mainCamera;

    private int player;

    // Start is called before the first frame update
    void Start()
    {
        /*
        foreach (Transform player in transform)
        {
        playerCharacters.Add(player.gameObject);
        }
        */

        player = PlayerPrefs.GetInt("personajeEscogido", 0);
        playerCharacters[player].SetActive(true);
        mainCamera = FindObjectOfType<CameraFollow>();
        mainCamera.target = playerCharacters[player];

    }

}
