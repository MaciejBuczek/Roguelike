using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveCreatureController : PassiveCreatureController
{
    public CharacterBars statBars;
    public Combat combat;
    public bool isFighting;

    protected int currentMana;

    private AggressiveCreature aggresiveCreature;

    protected override void Start()
    {
        aggresiveCreature = (AggressiveCreature)creature;
        currentMana = aggresiveCreature.mana.GetValue();
        statBars.SetMaxHealth(aggresiveCreature.health.GetValue());
        statBars.SetHealth(aggresiveCreature.health.GetValue());
        base.Start();
    }
    protected override void ChangeHealth(int amount)
    {
        if (Random.Range(1, 101) <= aggresiveCreature.dodge.GetValue())
        {
            Debug.Log(creature.name + " dodged");
            return;
        }
        base.ChangeHealth(amount);
        statBars.SetHealth(currentHealth);
    }
    protected virtual int GetAttackDamgae(IntRange damageSource)
    {
        int damage = damageSource.Random();
        if (Random.Range(1, 101) <= aggresiveCreature.critChance.GetValue())
            damage *= 2;
        return damage;
    }
    public int GetCurrentMana()
    {
        return currentMana;
    }
}
