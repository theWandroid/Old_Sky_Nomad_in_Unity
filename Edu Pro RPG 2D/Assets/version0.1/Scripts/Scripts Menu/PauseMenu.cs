using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject optionsScreen, pauseScreen;

    public string mainMenuScene;

    private bool isPaused;

    public GameObject loadingScreen, loadingIcon;
    public Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause() //bucle para pausar y quitar pausa
    {
        if (!isPaused)
        {
            pauseScreen.SetActive(true);
            isPaused = true;

            //pausing the game to say time should move at zero speed
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            isPaused = false;

            //unpausing the game to say time should move at one speed
            Time.timeScale = 1f;
        }
    }

    public void OpenOptions() //abrir menu opciones
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions() //cerrar menu opciones
    {
        optionsScreen.SetActive(false);
    }

    public void QuitToMain() //ir al menu principal
    {
        //SceneManager.LoadScene(mainMenuScene);

        //reset the time a timescale back to be one
        //Time.timeScale = 1f;

        StartCoroutine(LoadMain());

    }


    //Coroutine for loading screen
    public IEnumerator LoadMain()
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuScene);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if(asyncLoad.progress >= .9f)
            {
                loadingText.text = "Pulsa cualquier tecla para continuar";
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
