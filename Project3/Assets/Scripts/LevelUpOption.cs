using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class LevelUpOption : MonoBehaviour
{
    protected BaseUpgrade upgrade;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUpgrade(BaseUpgrade newUpgrade)
    {
        upgrade = newUpgrade;
    }

    public void ButtonClick()
    {
        // Apply upgrade
        upgrade.apply(Player.Instance);
    }
}
