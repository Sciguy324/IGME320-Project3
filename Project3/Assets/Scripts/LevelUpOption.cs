using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; 

public class LevelUpOption : MonoBehaviour
{
    protected Sprite img;
    protected string name;
    protected string upgradeInfo;
    protected Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        
    }

    public void ButtonClick(string upgradeName)
    {
        switch (upgradeName)
        {
            case "healthup":
                Player.Instance.maxHealth++;
                upgradeInfo = "Increases Henry's maximum health to allow him to take more punishment!";
                break;
            case "bulletnumber":
                Player.Instance.gun.maxMagazineSize++;
                upgradeInfo = "Increase the amount of charges that Henry can fire before reloading";
                break;
            default:
                break;
        }
    }
}
