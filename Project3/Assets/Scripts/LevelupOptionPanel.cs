using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelupOptionPanel : MonoBehaviour
{
    // public Image img;
    public TextMeshProUGUI text;
    public Image img;
    protected BaseUpgrade upgrade;

    public void SetUpgrade(BaseUpgrade newUpgrade)
    {   
        // Set name and image
        text.text = newUpgrade.UpgradeName;
        img.sprite = newUpgrade.img;

        // Set level up option
        upgrade = newUpgrade;
    }

    public void ButtonClick()
    {
        // Apply upgrade
        upgrade.apply(Player.Instance);

        // Resume time
        GameManager.Instance.HideLevelUpScreen();
    }
}
