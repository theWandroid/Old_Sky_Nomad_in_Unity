using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFunction : MonoBehaviour
{

    public string objectName;

    public GameObject objectCanvas;

    private CharacterStats stats;

   private void Start()
    {
        stats = GameObject.Find("Player").GetComponent<CharacterStats>();
    }
}
