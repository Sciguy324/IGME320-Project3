using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Upgrade", menuName="ScriptableObjects/Upgrades/BaseUpgrade")]
public class BaseUpgrade : ScriptableObject
{
    // Basic upgrade information
    public string UpgradeName = "Hello World";
    public string UpgradeDescription = "You are now breathing manually";
    
    // Temporary test variables
    public float speedModifier;
    public float shootingModifier;
}
