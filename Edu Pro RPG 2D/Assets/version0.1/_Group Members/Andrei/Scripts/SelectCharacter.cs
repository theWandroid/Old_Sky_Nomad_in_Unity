using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{

    public List<Sprite> characters;

    public List<GameObject> players;

    public TextMeshProUGUI text;

    public GameObject loadingScreen, loadingIcon;
    public TextMeshProUGUI loadingText;
    public string townScene;
    public GameObject selectPlayerPanel;


    public Button nextButton;


    public Button previousButton;

    public Image playerImage;

    public CameraFollow cameraFollow;

    private int num;

    private Image _image;




    // Start is called before the first frame update
    void Start()
    {
        cameraFollow = FindObjectOfType<CameraFollow>();
        cameraFollow.target = players[0];
        Debug.Log(num);
    }

    public void NextCharacter()
    {
        if (num < players.Count -1)
        {
        num++;
        }else if (num >= players.Count -1)
        {
            num = 0;
        }
        Debug.Log(num);

        ChangeTarget();
    }

    public void PreviousCharacter()
    {
        if (num > 0)
        {
        num--;
        } else if (num <= 0)
        {
            num = players.Count - 1;
        }
        Debug.Log(num);
        ChangeTarget();
    }

    public void ChoiceCharacter()
    {
        if (num >= 0 && num <= players.Count)
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

    private void ChangeTarget()
    {
        cameraFollow.target = players[num];
        players[num].GetComponent<SelectionPlayerMovement>().facingDirection = new Vector2(0, -1);
    }


   public IEnumerator LoadMain()
    {
        loadingScreen.SetActive(true);
        selectPlayerPanel.SetActive(false);
        //_image.color = new Color32(0, 0, 0, 0);

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
