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
        playerLevel.SetText(PlayerController.Instance.level.ToString());
    }
    public void ChangeLevelDetails()
    {
        PlayerController playerStats = PlayerController.Instance;
        //Change level details in character stats panel
        playerLevelDetails.SetText(playerStats.level + " " + PlayerController.Instance.GetCurrentExp() + "/" + PlayerController.Instance.nextLevelReq);
    }
    public void ChangeStatistics()
    {
        PlayerController playerController = PlayerController.Instance;

        //Change Attrubutes
        attributesUI.SetText(playerController.player.strength.GetValue() + "\n" + playerController.player.intelligence.GetValue() +
            "\n" + playerController.player.dexterity.GetValue());
        //Change damage melee, health, dodge, armor
        playerStats1.SetText(playerController.player.damageMelee.min + "-" + playerController.player.damageMelee.max + "\n" +
            playerController.GetCurrentHealth() + "/" + playerController.player.health.GetValue() + "\n" 
            + playerController.player.dodge.GetValue() + "%\n" + playerController.player.armor.GetValue());
        //Change damage ranged, mana, crit chance
        playerStats2.SetText(playerController.player.damageRanged.min + "-" + playerController.player.damageRanged.max + "\n" +
            playerController.GetCurrentMana() + "/" + playerController.player.mana.GetValue() + "\n" + playerController.player.critChance.GetValue() + "%");
    }
}
