using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManagerSingleton: MonoBehaviour
{
    private static SFXManagerSingleton sharedInstance = null;

    public static SFXManagerSingleton SharedInstance
    {
        get
        {
            return sharedInstance;
        }
    }


    private void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }

        sharedInstance = this;
        DontDestroyOnLoad(gameObject);


        audios = new List<GameObject>();

        GameObject sounds = GameObject.Find("Sounds");
        foreach (Transform t in sounds.transform)
        {
            audios.Add(t.gameObject);
        }
        Debug.LogFormat("Los audios son" + audios.Count);
    }

    private List <GameObject> audios;

    public AudioSource FindAudioSource(SFXType.SoundType type)
    {
        foreach(GameObject g in audios)
        {
            if(g.GetComponent<SFXType>().type == type)
            {
                return g.GetComponent<AudioSource>();
            }
        }
        return null;//esto no se ejecutara nunca
    }

    public void PlaySFX(SFXType.SoundType type)
    {
        FindAudioSource(type).Play();
    }

    public void StopSFX(SFXType.SoundType type){
        FindAudioSource(type).Stop();
    }
}
