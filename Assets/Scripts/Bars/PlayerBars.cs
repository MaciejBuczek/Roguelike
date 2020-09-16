using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : CharacterBars
{
    #region Singleton
    public static PlayerBars Instance;
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("More then one instance of PlayerBars found");
        else
            Instance = this;
    }
    #endregion
    public Slider manaBar, expBar;
    // Start is called before the first frame update
    public void SetMaxMana(int maxMana)
    {
        manaBar.maxValue = maxMana;
    }
    public void SetMana(int mana)
    {
        manaBar.value = mana;
    }
    public void SetMaxExp(int maxExp)
    {
        expBar.maxValue = maxExp;
    }
    public void SetExp(int exp)
    {
        expBar.value = exp;
    }
}
