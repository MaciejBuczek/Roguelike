using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public CharacterStats stats;
    protected void AttackMeele(Combat enemy)
    {
        int damage = stats.damageMelee.Random();
        DealDamage(enemy, damage);
    }
    protected void DealDamage(Combat enemy, int damage)
    {
        enemy.ReceiveDamage(damage);
    }
    protected void ReceiveDamage(int Damage)
    {
        if (Random.Range(0, 100) <= stats.dodge.GetValue())
            Debug.Log(transform.name + " dodged");
        else
            stats.TakeDamage(Damage);
    }
}
