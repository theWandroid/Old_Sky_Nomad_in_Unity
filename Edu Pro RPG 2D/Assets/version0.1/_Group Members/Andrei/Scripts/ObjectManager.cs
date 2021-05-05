using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private List<GameObject> objects;
    public int activeObject;


    //necesito todos los objetos
    public List<GameObject> GetAllObjects() {
        Debug.Log(objects.Count);
        return objects;
    }

    public ObjectFunction GetObjectAt(int pos)
    {
        return objects [pos].GetComponent<ObjectFunction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>();
        //Cuando hacemos una enumeración de transform estamos enumerando los hijos
        foreach(Transform obj in transform)
        {
            objects.Add(obj.gameObject);
        }

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
    }

    public void ChangeObject(int newObject)
    {//desactivo el objeto actual
        objects[activeObject].SetActive(false);
        //activo el nuevo objeto
        objects[newObject].SetActive(true);
        activeObject = newObject;

        //envia al metodo de canviar la imagen del avatar como parametro la imagen del arma actual
        //FindObjectOfType<UIManager>().ChangeAvatarImage(objects[activeObject].GetComponent<SpriteRenderer>().sprite);
    }


}
