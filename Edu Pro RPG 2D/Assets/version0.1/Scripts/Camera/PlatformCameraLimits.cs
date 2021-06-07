using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCameraLimits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CameraPlatform>().ChangeLimits(this.GetComponent<BoxCollider2D>());
    }
}
