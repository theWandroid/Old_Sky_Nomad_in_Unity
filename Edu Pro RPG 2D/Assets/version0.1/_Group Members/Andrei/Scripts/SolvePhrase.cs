using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolvePhrase : MonoBehaviour
{
    GameObject pieza;
    float distanciaRayo;
    bool estaPulsado;
    int ultimaFichaOrden = 1;
    int contadorSolucionesCorrectas = 0;
    bool permiteJugar = false;

    public List<GameObject> fichasMoviles;
    public List<GameObject> fichasSolucion;
    public GameObject canvas;

    private void Start()
    {
        Invoke("Desordenar", 2.0f);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && permiteJugar)
        {
            Debug.Log("He seleccionado la pieza");
            SeleccionarPieza();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SoltarPieza();
        }

        if (estaPulsado)
        {
            MoverPieza();
        }
    }

    void SeleccionarPieza()
    {
        //Si estamos trabajando con Colliders 2D
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        Debug.Log(hit);
        if (hit)
            if (hit.collider.gameObject.tag == "piezapuzzle")
            {
                ultimaFichaOrden++;
                estaPulsado = true;
                distanciaRayo = hit.distance; //distanciaRayo será un valor que utilizaremos más adelante
                pieza = hit.collider.gameObject;
                pieza.GetComponent<SpriteRenderer>().sortingOrder = ultimaFichaOrden;

                Debug.Log(pieza);
            }
        /*
         Si estamos trabajando con Colliders 3D
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(rayo, out hit)){
        ...
        }
        */
    }

    void MoverPieza()
    {
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 limiteRayo = rayo.GetPoint(distanciaRayo);
        limiteRayo = new Vector3(limiteRayo.x, limiteRayo.y, 0);
        pieza.transform.position = limiteRayo;
    }

    void SoltarPieza()
    {
        if (estaPulsado)
        {
            ComprobarDrop();
            estaPulsado = false;
            pieza = null;
        }
    }

    public void ComprobarDrop()
    {
        for (int i = 0; i < fichasSolucion.Count; i++)
        {
            if (pieza.name == fichasSolucion[i].name)
            {
                if (Vector2.Distance(pieza.transform.position, fichasSolucion[i].transform.position) < 1)
                {
                    pieza.transform.position = fichasSolucion[i].transform.position;
                    pieza.GetComponent<SpriteRenderer>().color = Color.green ;
                    Debug.Log(fichasMoviles.Count);

                    contadorSolucionesCorrectas++;
                    Debug.Log(contadorSolucionesCorrectas);
                    if (contadorSolucionesCorrectas == fichasMoviles.Count)
                    {
                        Debug.Log("Ganaste");
                        canvas.SetActive(true);
                    }
                    pieza.GetComponent<BoxCollider2D>().enabled = false;

                    break;
                }
            }
        }
    }


    void Desordenar()
    {
        for (int i = 0; i < fichasMoviles.Count; i++)
        {
            fichasMoviles[i].transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-2, 2), 0);
        }
        permiteJugar = true;
    }

}
