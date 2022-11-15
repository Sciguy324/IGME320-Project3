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
                break;
            case "bulletnumber":
                Player.Instance.gun.maxMagazineSize++;
                break;
            default:
                break;
        }
    }
}
