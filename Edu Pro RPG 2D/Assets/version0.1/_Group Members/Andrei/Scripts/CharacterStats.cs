using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour

{   //el valor maximo
    public const int MAX_STAT_VAL = 100;
    public const int MAX_HEALTH = 9999;

    [Tooltip("The level of the character")]
    public int level;
    [Tooltip("The experience of the character")]
    public int exp;
    //cuantos niveles habrá y cuanta experiencia será necesaria para subir de un nivel al siguiente
    [Tooltip("How many levels and how many experience I need to level up")]
    public int [] expToLevelUp;
    [Tooltip("How many health have the character in every level")]
    public int [] hpLevels;
    [Tooltip("How many strength the character have in each level")]
    public int [] strengthLevels;
    [Tooltip("How may defense have the character in each level")]
    public int[] defenseLevels;

    public int [] speedLevels;
    [Tooltip("Probabilidad de que el enemigo falle")]
    public int[] luckLevels;
    [Tooltip("Probabilidad de que falle el personaje")]
    public int[] accuracyLevels;

    private HealthManager healthManager;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        playerController = GetComponent<PlayerController>();

        healthManager.UpdateMaxHealth(hpLevels[level]);

        if (gameObject.tag.Equals("Enemy"))
        {
            EnemyController controller = GetComponent<EnemyController>();
            controller.speed += speedLevels[level] / MAX_STAT_VAL;
        }
    }
    /*
    // Update is called once per frame
    void Update()
    {
        //lo añadimos por seguridad, sino cuando llegasemos al ultimo nivel, la cosa petaria
        if(level >= expToLevelUp.Length)
        {
            return;
        }
        /*
        if (exp > expToLevelUp[level])
        {
            level++;
        }
        
        if(exp - expToLevelUp[level-1]>= expToLevelUp[level])
        {
            level++;
        }
    }
*/
    public void AddExperience(int exp)
    {
        this.exp += exp;
        //Debug.Log("Estoy dentro de AddExperience");
        if (level >= expToLevelUp.Length)
        {
            Debug.Log("No puedes subir más de nivel, ya estás en el máximo");
            return;
        }
        //if the experience is more than the experience need to level up
        if(this.exp>= expToLevelUp[level])
        {//we add a level
            Debug.LogFormat($"Vamos a subir de nivel ");
            level++;
            Debug.LogFormat($"Vamos a subir al nivel {level}");
            this.exp = 0;
            //and update the max health

            healthManager.UpdateMaxHealth(hpLevels[level]);
            //playerController.attackTime -= speedLevels[level]/MAX_STAT_VAL;
            
        }
    }
}
