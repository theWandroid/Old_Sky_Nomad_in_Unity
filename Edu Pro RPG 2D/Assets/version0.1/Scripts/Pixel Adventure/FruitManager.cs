using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class FruitManager : MonoBehaviour
{
    public string scene;
	public GameObject loadingScreen; 
	public GameObject loadingIcon;
	public TextMeshProUGUI loadingText;
	public GameObject winPanel;


    private void Update()
    {
        AllFruitsCollected();
    }
    public void AllFruitsCollected()
    {
        if (transform.childCount == 0)
        {
            Debug.Log("No quedan frutas, You Win!");
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			//SceneManager.LoadScene(scene);
			//StartCoroutine(Ut);
			winPanel.SetActive(true);
			StartCoroutine(Utilities.LoadTwoSeconds());
			StartCoroutine(ReturnToTown());

        }
    }

	public IEnumerator ReturnToTown()
	{
		loadingScreen.SetActive(true);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

		asyncLoad.allowSceneActivation = false;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= .9f)
			{
				loadingText.text = "Pulsa para continuar";
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
