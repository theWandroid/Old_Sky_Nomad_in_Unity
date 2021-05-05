using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManagerStandart : MonoBehaviour
{

    public enum SFXType
    {
    CURRUCA_CAPIROTADA,
    MIRLO_COMUN
    }

    public AudioSource currucaCapirotada;
    public AudioSource mirloComun;

    public void PlaySFX(SFXType type)
    {
        switch (type)
        {
            case SFXType.CURRUCA_CAPIROTADA:
                currucaCapirotada.Play();
                break;
            case SFXType.MIRLO_COMUN:
                mirloComun.Play();
                break;
            default:
                return;
        }
    }
}
