using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Gui : MonoBehaviour
{
    public TMP_Text timerText;
    float currentTime = 0;
    float maxTime;
    public bool timerIsRunning = true;
    public GameObject[] hearts;
    public Slider expBar;
    public TMP_Text expText;
    public TMP_Text ammoText;
    public bool isRealoding;
    public Slider reloadinBar;
    private float reloadTime;
    private float currentReloadProgress;

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
        if (isRealoding)
        {
            currentReloadProgress+=Time.deltaTime;
            reloadinBar.value = currentReloadProgress / reloadTime;
            if (currentReloadProgress > reloadTime)
            {
                isRealoding = false;
                ammoText.gameObject.SetActive(true);
                reloadinBar.gameObject.SetActive(false);
            }
        }
     

    }
    public void SetHelathUI(int currentHealth)
    {
        currentHealth--;
        if (currentHealth > 5)
            Debug.LogWarning("Too much health!");
        for (int i = 0; i < 6; i++)
        {
            if(currentHealth>=i)
            hearts[i].SetActive(true);
            else
                hearts[i].SetActive(false);


        }        
    }

    public void SetEXP(int exp, int maxEXP)
    {
        expBar.value = (float)exp / (float)maxEXP;
        expText.text = exp.ToString() + " / " + maxEXP.ToString();
    }
    public void SetAmmo(int maxAmmo, int curretAmmo)
    {
       ammoText.text = curretAmmo + " / " + maxAmmo ;
    }
    public void startReloading(float reload)
    {
        currentReloadProgress = 0;
        reloadTime = reload;
        isRealoding = true;
        ammoText.gameObject.SetActive(false);
        reloadinBar.gameObject.SetActive(true);

    }

}
