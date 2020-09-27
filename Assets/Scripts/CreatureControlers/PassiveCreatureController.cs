using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCreatureController : MonoBehaviour
{
    public PassiveCreature creature;
    public Movement movement;
    public bool isAvtive;
    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = creature.health.GetValue();
        movement.sightDistance = creature.sightDistance;
        isAvtive = false;
    }
    public virtual void MakeTurn()
    {
        
    }
    protected virtual void ChangeHealth(int amount)
    {
        amount -= creature.armor.GetValue();
        Mathf.Clamp(amount, 0, int.MaxValue);
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log(creature.name + " died");
    }
    public virtual void ApplyAnimator()
    {
        GetComponent<Animator>().runtimeAnimatorController = creature.mainAnimatorOverride;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
