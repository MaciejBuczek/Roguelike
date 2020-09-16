using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat : MonoBehaviour
{

    // Start is called before the first frame update
    public CharacterStats stats;

    public abstract void SetAnimationAttack();
    public abstract void SetAnimationHit();
    public abstract void SetAnimationDie();
    
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
        /*if (Random.Range(0, 100) <= stats.dodge.GetValue())
            Debug.Log(transform.name + " dodged");
        else
            stats.TakeDamage(Damage);*/
    }
}
