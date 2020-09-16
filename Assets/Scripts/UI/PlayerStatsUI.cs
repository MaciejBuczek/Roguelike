using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    #region Singleton
    public static PlayerStatsUI Instance;
    public void Awake()
    {
        if (Instance != null)
            Debug.LogError("More then one instance of PlayerStatsUI found");
        else
            Instance = this;
    }
    #endregion
    public TextMeshProUGUI playerStats1, playerStats2, playerLevelDetails, attributesUI, itemsUI, playerLevel;

    public void ChangeAll()
    {
        ChangeLevel();
        ChangeLevelDetails();
        ChangeStatistics();
    }
    public void ChangeLevel()
    {
        //Change player level
        playerLevel.SetText(PlayerStats.Instance.level.ToString());
    }
    public void ChangeLevelDetails()
    {
        PlayerStats playerStats = PlayerStats.Instance;
        //Change level details in character stats panel
        playerLevelDetails.SetText(playerStats.level + " " + playerStats.currentExp + "/" + playerStats.nextLevelExp);
    }
    public void ChangeStatistics()
    {
        PlayerStats playerStats = PlayerStats.Instance;

        //Change Attrubutes
        attributesUI.SetText(playerStats.strength.GetValue() + "\n" + playerStats.intelligence.GetValue() + "\n" + playerStats.dexterity.GetValue());
        //Change damage melee, health, dodge, armor
        playerStats1.SetText( playerStats.damageMelee.min + "-" + playerStats.damageMelee.max + "\n" + playerStats.currentHealth + "/" + playerStats.health.GetValue() + "\n" 
            + playerStats.dodge.GetValue() + "%\n" + playerStats.armor.GetValue());
        //Change damage ranged, mana, crit chance
        playerStats2.SetText(playerStats.damageRanged.min + "-" + playerStats.damageRanged.max + "\n" + playerStats.currentMana + "/" + playerStats.mana.GetValue() + "\n" +
           playerStats.critChance.GetValue() + "%");
    }
}
