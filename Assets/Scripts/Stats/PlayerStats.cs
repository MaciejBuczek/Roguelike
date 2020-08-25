using UnityEngine;
public class PlayerStats : CharacterStats
{
    public Stat strength, dexterity, intelligence;
    public int currentMana, currentExp = 0, nextLevelExp = 100, level;
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
        strength.SetBaseValue(10);
        dexterity.SetBaseValue(10);
        intelligence.SetBaseValue(10);
        CalculateAll();
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
    public override void TakeDamage(int damage)
    {
        int damageVal = damage - armor.GetValue();
        Mathf.Clamp(damageVal, 0, int.MaxValue);
        base.TakeDamage(damage);
        CharacterPanel.Instance.SubtractHealth(damageVal);
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
                    damageMelee.min = newItem.damage.min;
                    damageMelee.max = newItem.damage.max;
                }
                else
                {
                    damageRanged.min = newItem.damage.min;
                    damageRanged.max = newItem.damage.max;
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
                    damageMelee.min = unarmedDamage.min;
                    damageMelee.max = unarmedDamage.max;
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
