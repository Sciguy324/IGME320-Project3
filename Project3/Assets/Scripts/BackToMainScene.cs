using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToMainScene : MonoBehaviour
{
    public void BackToMain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
