using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName="Upgrade", menuName="ScriptableObjects/Upgrades/BaseUpgrade")]
public class BaseUpgrade : ScriptableObject
{
    // Basic upgrade information
    public string UpgradeName = "Hello World";
    public string UpgradeDescription = "You are now breathing manually";
    public Sprite img;

    // What effect does this upgrade have?
    public bool healthBoost = false;
    public int speedBoost = 0;
    public int DamageBoost = 0;
    public int shootingBoost = 0;
    public int magBoost = 0;
    public int piercingBoost = 0;
    public float reloadMultiplier = 1.0f;
    public bool unlockHat = false;

    public void apply(Player player)
    {
        // Apply the upgrades to the player
        if (healthBoost)
            player.addHeart();
        if (speedBoost > 0)
            player.speed += speedBoost;
        if (shootingBoost > 0)
            player.gun.bulletSpeed += shootingBoost;
        if (magBoost > 0)
            player.gun.expandMagazine(magBoost);
        if (piercingBoost > 0)
            player.gun.piercing++;
        if (DamageBoost > 0)
            player.gun.damage++;
        if (reloadMultiplier != 0.0f)
            player.reloadSpeed *= reloadMultiplier;
        if (unlockHat)
            player.unlockHat();
    }

    public bool getOk(Player player)
    {
        // Determines whether this upgrade is allowed
        // Health limit of 6
        if (healthBoost && player.getHealth() >= 6)
        {
            return false;
        }

        // Hat limit of 1
        if (unlockHat && player.wearingHat)
        {
            return false;
        }

        // All checks passed
        return true;
    }
}
