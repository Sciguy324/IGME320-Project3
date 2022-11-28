using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject onboardScreen;
    public GameObject creditsScreen;

    public void GoToCredits()
    {
        creditsScreen.SetActive(true);
        onboardScreen.SetActive(false);
        startScreen.SetActive(false);
        Debug.Log("Game is going to credits screen");
    }

    public void GoToHowToPlay()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(true);
        startScreen.SetActive(false);
        Debug.Log("Game is going to onboarding screen");
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
    }

    public void GoToStart()
    {
        creditsScreen.SetActive(false);
        onboardScreen.SetActive(false);
        startScreen.SetActive(true);
        Debug.Log("Game is going to start screen");
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
