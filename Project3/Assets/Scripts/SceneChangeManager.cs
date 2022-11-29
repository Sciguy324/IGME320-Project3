using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject onboardScreen;
    public GameObject creditsScreen;
    public GameObject pauseScreen;
    public Texture2D targetCursorTexture;
    public static SceneChangeManager Instance { get; private set; }

    public void GoToCredits()
    {
        creditsScreen.SetActive(true);
        onboardScreen.SetActive(false);
        startScreen.SetActive(false);
        pauseScreen.SetActive(false);
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

        Instance = this;
    }

    public void GoToHowToPlay()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(true);
        startScreen.SetActive(false);
        pauseScreen.SetActive(false);
        Debug.Log("Game is going to onboarding screen");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void GoToGame()
    {
        // If we are already in the scene just get rid of the pause screen
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            Player.Instance.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
        Debug.Log("Game is starting/resuming");
        Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void GoToStart()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(false);
        startScreen.SetActive(true);
        pauseScreen.SetActive(false);
        Debug.Log("Game is going to start screen");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    
    public void OnPause(InputValue value)
    {
        pauseScreen.SetActive(true);
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(false);
        startScreen.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0;
        Player.Instance.gameObject.SetActive(false);
        Debug.Log("Game is pausing");
    }
    

    public void QuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
