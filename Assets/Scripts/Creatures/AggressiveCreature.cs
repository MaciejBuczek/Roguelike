using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creatures/Aggressive")]

public class AggressiveCreature : PassiveCreature
{
    public Stat mana, dodge, critChance;
    public IntRange damageMelee, damageRanged;
    public int attackSpeedMelee, attackSpeedRanged;
}
