using UnityEngine;
using UnityEngine.UI;

public class CharacterBars : MonoBehaviour
{
    public Slider healthBar;
    // Start is called before the first frame update

    public void SetMaxHealth(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
    }
    public void SetHealth(int health)
    {
        healthBar.value = health;
    }
}
