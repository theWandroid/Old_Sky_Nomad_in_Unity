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

    public Button nextButton;

    public Button previousButton;

    public Image playerImage;

    private int num;

    // Start is called before the first frame update
    void Start()
    {
        playerImage.sprite = characters[0];
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
                characterChoice = "Has escogido a la mujer";
            }
            else if (num == 1)
            {
                characterChoice = "Has escogido al viejo";
            }
            else if (num == 2)
            {
                characterChoice = "Has escogido al/la caballer@";
            }
            PlayerPrefs.SetInt("personajeEscogido", num);
            text.text = characterChoice;
            PlayerPrefs.GetInt("personajeEscogido", 0);
            Debug.Log("Se ha escogido a " +PlayerPrefs.GetInt("personajeEscogido", 0));
            SceneManager.LoadScene("town");
        }
       
        else
        {
            Debug.Log("El personaje no esta dentro de las opciones");
        }

    
    }
}
