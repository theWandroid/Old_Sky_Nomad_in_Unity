using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Mireia : MonoBehaviour
{
    public GameObject texto;
    public GameObject boolean;
    public GameObject number;
    public String mireia1;
    public Boolean isWalking;
    public float numero = 5;
    private String mireia;
    // Start is called before the first frame update
    void Start()
    {
        mireia = "lol";
        Debug.Log("Hey! I'm Mireia");
        texto.GetComponent<TextMeshProUGUI>().text = "Hello there";
        number.GetComponent<TextMeshProUGUI>().text=  numero.ToString(); 
        boolean.GetComponent<TextMeshProUGUI>().text = isWalking.ToString();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
