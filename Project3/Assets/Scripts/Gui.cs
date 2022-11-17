using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Gui : MonoBehaviour
{
    public TMP_Text timerText;
    float currentTime = 0;
    float maxTime;
    public bool timerIsRunning = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (timerIsRunning == true)
        {

            currentTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentTime / 60F);
            int seconds = Mathf.FloorToInt(currentTime % 60F);
            int milliseconds = Mathf.FloorToInt((currentTime * 100F) % 100F);
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        }

    }
}
