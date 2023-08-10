using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private float displayDuration = 4.0f; // Duration for which the start text is displayed

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            ShowStartText();
        }
    }

    // Start the game by advancing to the next scene
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit the game when the Quit button is pressed
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    // Show the start text and hide it after the specified duration
    public void ShowStartText()
    {
        StartCoroutine(DisplayStartTextAndHide());
    }

    // Coroutine to display the start text and hide it after a delay
    private IEnumerator DisplayStartTextAndHide()
    {
        yield return new WaitForSeconds(displayDuration);

        startText.enabled = false;
    }
}