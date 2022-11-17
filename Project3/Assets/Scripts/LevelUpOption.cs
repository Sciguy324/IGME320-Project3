using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; 

public class LevelUpOption : MonoBehaviour
{
    protected Sprite img;
    public string name;
    protected string upgradeInfo;
    protected Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        
    }

    public void ButtonClick()
    {
        switch (name)
        {
            case "healthup":
                Player.Instance.maxHealth++;
                upgradeInfo = "Increases Henry's maximum health to allow him to take more punishment!";
                break;
            case "bulletnumber":
                Player.Instance.gun.maxMagazineSize++;
                upgradeInfo = "Increase the amount of charges that Henry can fire before reloading";
                break;
            case "playerspeed":
                Player.Instance.speed++;
                break;
            case "bulletspeed":
                Player.Instance.gun.bulletSpeed++;
                break;
            default:
                break;
        }
    }
}
