using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public IntRange damageMelee, damageRanged;
    public Stat armor, dodge, health, mana, critChance;
    public Slider healthBar;
    public int currentHealth, currentMana;

    protected virtual void Start()
    {
        currentHealth = health.GetValue();
        currentMana = mana.GetValue();
        healthBar.maxValue = health.GetValue();
        healthBar.value = health.GetValue();
    }
    public virtual void ChangeHealth(int health)
    {
        health -= armor.GetValue();
        Mathf.Clamp(health, 0, int.MaxValue);
        currentHealth -= health;

        //do health calc
        if (currentHealth <= 0)
            Die();
        else
            healthBar.value =currentHealth;
    }
    public virtual void Die()
    {
        Debug.Log(transform.name + " Died");
    }
}
