using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    #region Singelton
    public static CharacterPanel Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("More then one instance of CharacterPanel found");
            return;
        }
        Instance = this;
    }
    #endregion
    enum StatBarsId { health, mana, exp};
    public Slider[] statBars;
    public TextMeshProUGUI characterLevel;

    public void SetMaxHP(int maxHp)
    {
        statBars[(int)StatBarsId.health].maxValue = maxHp;
    }
    public void SetMaxMana(int maxMana)
    {
        statBars[(int)StatBarsId.mana].maxValue = maxMana;
    }
    public void SetMaxExp(int maxExp)
    {
        statBars[(int)StatBarsId.exp].maxValue = maxExp;
    }
    public void SubtractHealth(int health)
    {
        statBars[(int)StatBarsId.health].value -= health;
    }
    public void AddHealth(int health)
    {
        statBars[(int)StatBarsId.health].value += health;
    }
    public void SetHealth(int health)
    {
        statBars[(int)StatBarsId.health].value = health;
    }
    public void SubtractMana(int mana)
    {
        statBars[(int)StatBarsId.mana].value -= mana;
    }
    public void AddMana(int mana)
    {
        statBars[(int)StatBarsId.mana].value += mana;
    }
    public void SetMana(int mana)
    {
        statBars[(int)StatBarsId.mana].value = mana;
    }
    public void AddExp(int exp)
    {
        statBars[(int)StatBarsId.exp].value += exp;
    }
    public void SetExp(int exp)
    {
        statBars[(int)StatBarsId.exp].value = exp;
    }
    public void SetLevel(int level)
    {
        characterLevel.text = level.ToString();
    }
}
