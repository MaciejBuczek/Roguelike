using UnityEngine;
public class PlayerStats : CharacterStats
{
    public Stat strength, dexterity, intelligence;
    public int currentMana, currentExp = 0, nextLevelExp = 100, level = 1;
    private IntRange unarmedDamage = new IntRange(1,2);
    public delegate void OnStatsChanged();
    public OnStatsChanged onStatsChanged;
    public static PlayerStats instance;

    // Start is called before the first frame update
    void Awake()
    {
        Equipment.instance.onEquipmentChanged += OnEquipmentChanged;
        if (instance != null)
        {
            Debug.Log("more then one instance of player stats found");
            return;
        }
        instance = this;

        SetBaseValues();

        CalculateAll();
        currentHealth = health.GetValue();
        currentMana = mana.GetValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(2);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            UseMana(2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GainExp(2);
        }
    }
    public void UseMana(int mana)
    {
        currentMana -= mana;
        CharacterPanel.Instance.SubtractMana(mana);
    }
    public void GainExp(int exp)
    {
        currentExp += exp;
        CharacterPanel.Instance.AddExp(exp);
    }
    private void CalculateHealth()
    {
        health.SetBaseValue(strength.GetValue() * 2);
    }
    private void CalculateDodge()
    {
        dodge.SetBaseValue(dexterity.GetValue());
    }
    private void CalculateMana()
    {
        mana.SetBaseValue(intelligence.GetValue() * 2);
    }
    private void CalculateAll()
    {
        CalculateHealth();
        CalculateDodge();
        CalculateMana();
        if(onStatsChanged != null)
        {
            onStatsChanged.Invoke();
        }
    }
    private void SetBaseValues()
    {
        strength.SetBaseValue(10);
        dexterity.SetBaseValue(10);
        intelligence.SetBaseValue(10);
        critChance.SetBaseValue(10);
        damageMelee = unarmedDamage;
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
        if (onStatsChanged != null)
        {
            onStatsChanged.Invoke();
        }
    }
    
}
