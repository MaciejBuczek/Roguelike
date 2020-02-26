using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public IntRange damageMelee, damageRanged;
    public Stat armor, dodge, health;
    public int currentHealth;

    private void Start()
    {
        currentHealth = health.GetValue();
    }
    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }
    public virtual void Die()
    {
        Debug.Log(transform.name + " Died");
    }
}
