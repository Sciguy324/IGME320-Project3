using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject onboardScreen;
    public GameObject creditsScreen;
    public Texture2D targetCursorTexture;

    public void GoToCredits()
    {
        creditsScreen.SetActive(true);
        onboardScreen.SetActive(false);
        startScreen.SetActive(false);
        Debug.Log("Game is going to credits screen");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    void Start()
    {
        // For when we're not starting on the "start" menu
        /*
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
        }
        */
    }

    public void GoToHowToPlay()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(true);
        startScreen.SetActive(false);
        Debug.Log("Game is going to onboarding screen");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void GoToGame()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(false);
        startScreen.SetActive(false);
        // Make player active so bullets work and stuff
        Player.Instance.gameObject.SetActive(true);
        // Resume the game
        Time.timeScale = 1;
        Debug.Log("Game is starting");
        Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void GoToStart()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(false);
        startScreen.SetActive(true);
        Debug.Log("Game is going to start screen");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
