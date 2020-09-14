using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{

    public TextMeshProUGUI playerStats1, playerStats2, playerExp, AttributesUI, ItemsUI;

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.instance.onStatsChanged += OnStatsChanged;
        OnStatsChanged();
    }

    public void OnStatsChanged()
    {
        PlayerStats playerStats = PlayerStats.instance;

        //Change Attrubutes
        AttributesUI.SetText(playerStats.strength.GetValue() + "\n" + playerStats.intelligence.GetValue() + "\n" + playerStats.dexterity.GetValue());
        //Display exp
        playerExp.SetText(playerStats.level + " " + playerStats.currentExp + "/" + playerStats.nextLevelExp);
        //Display damage melee, health, dodge, armor
        playerStats1.SetText( playerStats.damageMelee.min + "-" + playerStats.damageMelee.max + "\n" + playerStats.currentHealth + "/" + playerStats.health.GetValue() + "\n" 
            + playerStats.dodge.GetValue() + "%\n" + playerStats.armor.GetValue());
        //Display damage ranged, mana, crit chance
        playerStats2.SetText(playerStats.damageRanged.min + "-" + playerStats.damageRanged.max + "\n" + playerStats.currentMana + "/" + playerStats.mana.GetValue() + "\n" +
           playerStats.critChance.GetValue() + "%");
    }
}
