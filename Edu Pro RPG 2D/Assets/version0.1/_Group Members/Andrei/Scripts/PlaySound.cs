using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public SFXType.SoundType soundName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
        SFXManagerSingleton.SharedInstance.PlaySFX(soundName);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
        SFXManagerSingleton.SharedInstance.StopSFX(soundName);
        }
    }
}
