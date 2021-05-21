using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{

    // public string scene;

    public void Start()
    {
        StartCoroutine(LoadConnect());
    }

    //Coroutine for loading screen
    public IEnumerator LoadConnect()
    {
            yield return new WaitForSeconds(3f);
        // SceneManager.LoadScene(scene);
        SceneManager.LoadScene("Connect");
    }


}
