using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //The game starts by pressing the play button (skip to the next scene)
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //The game closes when the Quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}