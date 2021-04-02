using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CameraFollow>().ChangeLimits(this.GetComponent<BoxCollider2D>());
    }

}
