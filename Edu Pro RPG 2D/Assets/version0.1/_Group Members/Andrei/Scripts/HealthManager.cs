using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IManager, IHealth
{
    //implementamos la interface IManager i IHealth
    //cada clase que implementa una interface dara su version
    private string _state;
    public string State
    {
        get{ return _state; }
        set { _state = value; }
    }

    public void Initialize()
    {
        _state = "Health Manager inicializado";
        Debug.Log(_state);
    }
    //permite ver una variable privada
    [SerializeField]
    private int currentHealth;
    // Start is called before the first frame update
    public int Health
    {
        //la hacemos de solo lectura, sin el setter
        get
        {
            return currentHealth;
        }
        set
        {
            if (value < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }
        }
    }
    public int maxHealth;

    public bool flashActive;
    public float flashLength;
    private float flashCounter;

    private SpriteRenderer _characterRenderer;

    public int expWhenDefeated;

    private QuestEnemy quest;
    private QuestManager questManager;

    //implementamos la interface IManager
    

    void Start()
    {
        Initialize();
        _characterRenderer = GetComponent<SpriteRenderer>();
        //currentHealth = maxHealth;
        UpdateMaxHealth(maxHealth);
        //este es el metodo que hacemos servir cuando queremos comunicarnos con una componente que esta bastante extendida
        quest = GetComponent<QuestEnemy>();
        questManager = FindObjectOfType<QuestManager>();
    }

    

    public void DamageCharacter(int damage)
    {
        //SFXManagerSingleton.SharedInstance.PlaySFX(SFXType.SoundType.HIT);
        Health -= damage;
        if(Health <= 0)
        {
            //si acabamos de derrotar a un gameobject que tenga una etiqueta de enemigo
            if (gameObject.tag.Equals("Enemy"))
            {//localizamos al player y al metodo de añadir experiencia le pasamos como parametro los puntos de experiencia que tiene el enemigo cuando es eliminado
                Debug.Log("He matado a un enemigo");
                GameObject.Find("Player").GetComponent<CharacterStats>().AddExperience(expWhenDefeated);
                questManager.enemyKilled = quest;
            }

            if (gameObject.name.Equals("Player"))
            {
                //SFXManagerSingleton.SharedInstance.PlaySFX(SFXType.SoundType.DIE);
                Utilities.Freeze();
                Utilities.RestartLevel();
            }
            gameObject.SetActive(false);
        }
        if(flashLength > 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            //GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerController>().canMove = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            flashActive = true;
            flashCounter = flashLength;
        }
    }

    //this method get the current heath by the parameter that is given to him
    public void UpdateMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        Health = maxHealth;
    }

    void ToggleColor(bool visible)
    {
        _characterRenderer.color = new Color(_characterRenderer.color.r, _characterRenderer.color.g, _characterRenderer.color.b, (visible ? 1.0f : 0.0f));
    }

    // Update is called once per frame
    private void Update()
    {
        if (flashActive)
        {
            flashCounter -= Time.deltaTime;
            if(flashCounter > flashCounter * 0.66f)
            {
                ToggleColor(false);
            }else if(flashCounter > flashLength * 0.33f)
            {
                ToggleColor(true);
            }
            else if(flashCounter>0)
            {
                ToggleColor(false);
            }
            else
            {
                ToggleColor(true);
                flashActive = false;
                GetComponent<BoxCollider2D>().enabled = true;
                //GetComponent<PlayerController>().enabled = true;
                GetComponent<PlayerController>().canMove = true;

            }
        }
    }


}
