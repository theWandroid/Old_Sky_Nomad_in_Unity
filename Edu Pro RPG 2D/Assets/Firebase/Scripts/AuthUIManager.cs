using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AuthUIManager : MonoBehaviour
{
    public static AuthUIManager instance;

    [Header("References")]
    [SerializeField]
    private GameObject chekingForAccountUI;
    [SerializeField]
    private GameObject loginUI;
    [SerializeField]
    private GameObject registerUI;
    [SerializeField]
    private GameObject verifyEmailUI;
    [SerializeField]
    private TMP_Text verifyEmailText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ClearUI()
    {
        FirebaseManager.instance.ClearOutputs();
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        verifyEmailUI.SetActive(false);
   /*     checkingForAccountUI.SetActive(false);*/

    }

    public void LoginScreen()
    {
        ClearUI();
        loginUI.SetActive(true);
    }

    public void RegisterScreen()
    {
        ClearUI();
        registerUI.SetActive(true);
    }

    //Esperando Verificación
    public void AwaitVerification(bool _emailSent, string _email, string _output)
    {
        ClearUI();
        verifyEmailUI.SetActive(true);
        if (_emailSent)
        {
            verifyEmailText.text = $"Email Enviado \nPorfavor Verifica {_email}";
        }
        else
        {
            verifyEmailText.text = $"Email No Enviado: {_output}\nPorfavor Verifica {_email}";
        }

    }
}
