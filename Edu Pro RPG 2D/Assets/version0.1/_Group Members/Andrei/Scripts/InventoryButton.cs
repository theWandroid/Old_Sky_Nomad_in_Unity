using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public enum ItemType { POTION = 0, FOOD = 1, CLOTHES = 2, OTHER = 3 }

    public int itemIdx;
    public ItemType type;

    public void ActivateButton(){

        switch (type)
        {
            case ItemType.OTHER:
                FindObjectOfType<ObjectManager>().ChangeObject(itemIdx);
                break;

        
        }

    }
}
