using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public IntRange damageMelee, damageRanged;
    public Stat armor, dodge, health, mana;
    public int currentHealth;
    public Image healthBar;
    [SerializeField] private Canvas healthBarCanvas;
    private bool isHealthBarEnabled = true;

    private void Start()
    {
        currentHealth = health.GetValue();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHealthBarEnabled = !isHealthBarEnabled;
            if (isHealthBarEnabled)
                healthBarCanvas.enabled = true;
            else
                healthBarCanvas.enabled = false;
        }
    }
    public virtual void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        Mathf.Clamp(damage, 0, int.MaxValue);
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth/health.GetValue();
        if (currentHealth <= 0)
            Die();
    }
    public virtual void Die()
    {
        Debug.Log(transform.name + " Died");
    }
    private void OnMouseOver()
    {
        if (!isHealthBarEnabled)
            healthBarCanvas.enabled = true;
    }
    private void OnMouseExit()
    {
        if (!isHealthBarEnabled)
            healthBarCanvas.enabled = false;
    }
}
