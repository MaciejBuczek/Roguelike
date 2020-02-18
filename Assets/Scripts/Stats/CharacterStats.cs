using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private int maxHealth=100;
    private int currentHealth;

    public Stat armor;
    public Stat damage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void takeDamage(int damage)
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
