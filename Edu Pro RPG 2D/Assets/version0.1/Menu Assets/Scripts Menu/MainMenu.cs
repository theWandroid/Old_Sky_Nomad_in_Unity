using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public string firstLevel;

    public GameObject optionsScreen;

    public GameObject loadingScreen, loadingIcon;
    public Text loadingText;

    public void StartGame() //iniciar juego
    {
        //SceneManager.LoadScene(firstLevel);
        StartCoroutine(LoadStart());
    }

public void OpenOptions() //abrir menu opciones
    {
        optionsScreen.SetActive(true);
    }

public void CloseOptions() //cerrar menu opciones
    {
        optionsScreen.SetActive(false);
    }

public void QuitGame() //salir del juego
    {
        Application.Quit(); 
        Debug.Log("Salir");
    }
    

    //Coroutine for loading screen
    public IEnumerator LoadStart()
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(firstLevel);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= .9f)
            {
                loadingText.text = "Press any key to continue";
                loadingIcon.SetActive(false);

                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;

                    Time.timeScale = 1f;
                }
            }

            yield return null;
        }
    }
}
