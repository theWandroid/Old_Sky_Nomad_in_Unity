using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{

    public List<Sprite> characters;

    public TextMeshProUGUI text;

    public GameObject loadingScreen, loadingIcon;
    public TextMeshProUGUI loadingText;
    public string townScene;
    public GameObject selectPlayerPanel;


    public Button nextButton;


    public Button previousButton;

    public Image playerImage;

    private int num;

    private Image _image;


    // Start is called before the first frame update
    void Start()
    {
        playerImage.sprite = characters[0];
        _image = this.gameObject.GetComponent<Image>();
        Debug.Log(num);
    }

    public void NextCharacter()
    {
        if (num < characters.Count -1)
        {
        num++;
        Debug.Log(num);
        playerImage.sprite = characters[num];
        }
    }

    public void PreviousCharacter()
    {
        if (num > 0)
        {
        num--;
        Debug.Log(num);
        playerImage.sprite = characters[num];
        }
    }

    public void ChoiceCharacter()
    {
        if (num >= 0 && num <= characters.Count)
        {
            string characterChoice = "";
            if (num == 0)
            {
                Debug.Log("Has escogido a la mujer");
            }
            else if (num == 1)
            {
                Debug.Log("Has escogido al viejo");
            }
            else if (num == 2)
            {
                Debug.Log("Has escogido a caballer@");
            }

            characterChoice = "Tu selección esta hecha";
            PlayerPrefs.SetInt("personajeEscogido", num);
            text.text = characterChoice;
            PlayerPrefs.GetInt("personajeEscogido", 0);
            Debug.Log("Se ha escogido a " +PlayerPrefs.GetInt("personajeEscogido", 0));
            SceneManager.LoadScene("town");
          StartCoroutine(LoadMain());
        }
       
        else
        {
            Debug.Log("El personaje no esta dentro de las opciones");
        }

    
    }


   public IEnumerator LoadMain()
    {
        loadingScreen.SetActive(true);
        selectPlayerPanel.SetActive(false);
        _image.color = new Color32(0, 0, 0, 255);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(townScene);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= .9f)
            {
                loadingText.text = "Pulsa para continuar";
                PlayerPrefs.GetInt("personajeEscogido", 0);
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
