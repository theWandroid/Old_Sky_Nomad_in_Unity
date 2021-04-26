using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{

    public SFXType.SoundType soundName;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        SFXManagerSingleton.SharedInstance.PlaySFX(soundName);
        Debug.Log(soundName);
    }

    private void OnTriggerExit2D(Collider2D collision){
        SFXManagerSingleton.SharedInstance.StopSFX(soundName);
    }
}
