using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public Texture2D targetCursorTexture;

    void Start()
    {
        // For when we're not starting on the "start" menu
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("SampleScene");
        Cursor.SetCursor(targetCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void GoToStart()
    {
        SceneManager.LoadScene("StartMenu");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void QuitGame()
    {
        
    }
}
