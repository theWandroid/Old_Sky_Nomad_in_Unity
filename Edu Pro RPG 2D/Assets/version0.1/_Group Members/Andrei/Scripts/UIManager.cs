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
    //public Image playerAvatar;
    public GameObject inventoryPanel;
    public GameObject menuPanel;
    public Button inventoryButton;

    public QuestManager questManager;

    private ObjectManager objectManager;

    public GameObject confirmationPannel;

    private void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
        confirmationPannel.SetActive(false);
        inventoryPanel.SetActive(false);
        menuPanel.SetActive(false);
        questManager = FindObjectOfType<QuestManager>();
    }


    // Update is called once per frame
    void Update()
    {
/*
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        
        //playerHealthBar.maxValue = playerHealthManager.maxHealth;
        //playerHealthBar.value = playerHealthManager.Health;

        //StringBuilder funciona por el patron del contructor de programacion
        StringBuilder stringBuilder = new StringBuilder().
            //Asi es como funciona el patron del constructor
            Append("HP: ").
            Append(playerHealthManager.Health).
            Append(" / ").
            Append(playerHealthManager.maxHealth);
        //Transformamos el stringBuider en texto porque es un objeto
        playerHealthText.text = stringBuilder.ToString();
        */
    }

    //este metodo se encarga de canviar la imagen del avatar

    public void ToggleInventory()
    {

        Debug.Log("He pulsado el boton");
        // asi se cumple el efecto panel
        //si esta activo devuelve True pero con ! se desactiva, de la misma manera al revés

        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        menuPanel.SetActive(!menuPanel.activeInHierarchy);

        if (inventoryPanel.activeInHierarchy)
        {
            foreach (Transform t in inventoryPanel.transform)
            {
                Destroy(t.gameObject);
            }
            FillInventory();
        }
    }


    public void ActiveConfirmation( int idQuest)
    {
        Debug.Log("Se ha activado el panel de confirmación.");
        confirmationPannel.SetActive(true);
        questManager.actualQuest = idQuest;
            }

    public void DesactiveConfirmation()
    {
        confirmationPannel.SetActive(false);
    }

    public void FillInventory()
    {
        //ObjectManager manager = FindObjectOfType<ObjectManager>();
        //De esta manera recupero todos los objetos
        List<GameObject> objetos = objectManager.GetAllObjects();
        int i = 0;
        foreach (GameObject obj in objetos)
        {
            //instanciamos el inventory Button como hijo del Panel de inventario

            Button tempB = Instantiate(inventoryButton, inventoryPanel.transform);
            tempB.GetComponent<InventoryButton>().type = InventoryButton.ItemType.OTHER;
            tempB.GetComponent<InventoryButton>().itemIdx = i;
            tempB.onClick.AddListener(() => tempB.GetComponent<InventoryButton>().ActivateButton());
            tempB.image.sprite = obj.GetComponent<SpriteRenderer>().sprite;
            i++;
        }
    }

    //para mostrar elementos de un cierto tipo
    public void ShowOnly(int type){
        foreach (Transform t in inventoryPanel.transform)
        {
            t.gameObject.SetActive((int)t.GetComponent<InventoryButton>().type == type);
        }
    }

    public void ShowAll() {
        foreach (Transform t in inventoryPanel.transform){
            t.gameObject.SetActive(true);
        }
    }

    /*
public void ChangeAvatarImage(Sprite sprite)
{
    playerAvatar.sprite = sprite;
}
*/

    public void HealthChanged(){
    
    }

    public void LevelChanged(){
    
    }

    public void ExpChanged(){
    
    }


    public void Quit()
    {
        Application.Quit();
    }
}
