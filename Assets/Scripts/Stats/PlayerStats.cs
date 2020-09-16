using UnityEngine;
public class PlayerStats : CharacterStats
{
    public Stat strength, dexterity, intelligence;
    public int currentExp = 0, nextLevelExp = 100, level = 1;
    private IntRange unarmedDamage = new IntRange(1,5);

    #region Singleton
    public static PlayerStats Instance;

    void Awake()
    {
        if (Instance != null)
            Debug.Log("more then one instance of player stats found");
        else
            Instance = this;
    }
    #endregion
    
    protected override void Start()
    {
        Equipment.instance.onEquipmentChanged += OnEquipmentChanged;
        CalculateAll();
        PlayerBars.Instance.SetMaxExp(nextLevelExp);
        damageMelee = unarmedDamage;
        critChance.SetBaseValue(10);
        PlayerStatsUI.Instance.ChangeAll();
        base.Start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeHealth(2);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeMana(2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeExp(2);
        }
    }
    
    public void ChangeMana(int mana)
    {
        currentMana -= mana;
        //CharacterPanel.Instance.SubtractMana(mana);
        PlayerBars.Instance.SetMana(currentMana);
    }
    public void ChangeExp(int exp)
    {
        currentExp += exp;
        //CharacterPanel.Instance.AddExp(exp);
        PlayerBars.Instance.SetExp(currentExp);
    }
    private void CalculateHealth()
    {
        health.SetBaseValue(strength.GetValue() * 2);
        currentHealth = health.GetValue();
    }
    private void CalculateDodge()
    {
        dodge.SetBaseValue(dexterity.GetValue());
    }
    private void CalculateMana()
    {
        mana.SetBaseValue(intelligence.GetValue() * 2);
        currentMana = mana.GetValue();
        PlayerBars.Instance.SetMaxMana(mana.GetValue());
    }
    private void CalculateAll()
    {
        CalculateHealth();
        CalculateDodge();
        CalculateMana();
        PlayerStatsUI.Instance.ChangeStatistics();
    }
    void OnEquipmentChanged(Equippable newItem, Equippable oldItem)
    {
        if (newItem != null)
        {
            if (newItem.armorModifier != 0)
                armor.AddModifier(newItem.armorModifier);
            else if (newItem.inventorySlotType == InventorySlotType.MeleeWeapon || newItem.inventorySlotType == InventorySlotType.RangedWeapon)
            {
                if (newItem.inventorySlotType == InventorySlotType.MeleeWeapon)
                {
                    damageMelee = newItem.damage;
                    critChance.SetBaseValue(newItem.critChance);
                }
                else
                {
                    damageRanged = newItem.damage;
                }
            }
        }
        else
        {
            if(oldItem.armorModifier!=0)
                armor.RemoveModifier(oldItem.armorModifier);
            else if (oldItem.inventorySlotType == InventorySlotType.MeleeWeapon || oldItem.inventorySlotType == InventorySlotType.RangedWeapon)
            {
                if (oldItem.inventorySlotType == InventorySlotType.MeleeWeapon)
                {
                    damageMelee = unarmedDamage;
                    critChance.SetBaseValue(10);
                }
                else
                {
                    damageRanged.min = 0;
                    damageRanged.max = 0;
                }
            }
        }
        PlayerStatsUI.Instance.ChangeStatistics();
    }
    
}
