using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzleManager : MonoBehaviour
{
    public GameObject loadingScreen, loadingIcon;
	public string returnScene;
	public TextMeshProUGUI loadingText;

	
	public void Return()
	{

		StartCoroutine(ReturnToMain());
		//SceneManager.LoadScene("town");
	}

	public IEnumerator ReturnToMain()
	{
		loadingScreen.SetActive(true);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(returnScene);

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
