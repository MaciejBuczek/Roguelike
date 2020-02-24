using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Text[] statsText;

    private enum StatsID {health, mana, dodge, strength, intelligence, dexterity, damageMelee, damageRanged, armor};

    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.instance.onStatsChanged += OnStatsChanged;
        OnStatsChanged();
    }

    public void OnStatsChanged()
    {
        PlayerStats playerStats = PlayerStats.instance;
        statsText[(int)StatsID.health].text = playerStats.currentHealth + "/" + playerStats.health.GetValue();
        statsText[(int)StatsID.mana].text = playerStats.mana.GetValue().ToString();
        statsText[(int)StatsID.dodge].text = playerStats.dodge.GetValue() + "%";
        statsText[(int)StatsID.strength].text = playerStats.strength.GetValue().ToString();
        statsText[(int)StatsID.intelligence].text = playerStats.intelligence.GetValue().ToString();
        statsText[(int)StatsID.dexterity].text = playerStats.dexterity.GetValue().ToString();
        statsText[(int)StatsID.damageMelee].text = playerStats.damageMelee.min + "-" + playerStats.damageMelee.max;
        statsText[(int)StatsID.damageRanged].text = playerStats.damageRanged.min + "-" + playerStats.damageRanged.max;
        statsText[(int)StatsID.armor].text = playerStats.armor.GetValue().ToString();
    }
}
