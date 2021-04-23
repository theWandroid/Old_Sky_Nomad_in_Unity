using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIManager : MonoBehaviour
{

    public Slider playerHealthBar;
    public TextMeshProUGUI playerHealthText;
    public HealthManager playerHealthManager;


    // Update is called once per frame
    void Update()
    {
        playerHealthBar.maxValue = playerHealthManager.maxHealth;
        playerHealthBar.value = playerHealthManager.Health;

        //StringBuilder funciona por el patron del contructor de programacion
        StringBuilder stringBuilder = new StringBuilder().
            //Asi es como funciona el patron del constructor
            Append("HP: ").
            Append(playerHealthManager.Health).
            Append(" / ").
            Append(playerHealthManager.maxHealth);
        //Transformamos el stringBuider en texto porque es un objeto
        playerHealthText.text = stringBuilder.ToString();
    }
}
