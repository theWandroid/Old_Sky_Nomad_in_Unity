using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//cargamos la libreria, el namespace para que pueda encargarse de cargar las escenas
using UnityEngine.SceneManagement;

//es una caja de herramientas que servirá para todo el juego, solo habrá una para todo el juego
//una clase estatica no puede heredar de nadie, es una clase que será compartida por todo el mundo
//lo contrario a static es dinamica, por defecto las clases son dinamicas
//esta clase se encargará de todo, estará por encima
public static class Utilities 
{
    //saber cuantas muertes lleva el jugador
    public static int playerDeaths = 0;
    public static Animator animator;
    public static PlayerController playerController;

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public static void Freeze()
    {
        Time.timeScale = 0.0f;
    }

    public static void Resume()
    {
        Time.timeScale = 1.0f;
    }

    public static IEnumerator LoadTwoSeconds()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
