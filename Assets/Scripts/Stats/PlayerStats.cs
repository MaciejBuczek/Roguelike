using UnityEngine;
public class PlayerStats : CharacterStats
{
    public Stat mana, strength, dexterity, intelligence;

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
            if (newItem.inventorySlotType == InventorySlotType.MeleeWeapon || newItem.inventorySlotType == InventorySlotType.RangedWeapon)
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
        if (oldItem != null)
        {
            if (oldItem.armorModifier != 0) 
                armor.RemoveModifier(oldItem.armorModifier);
            if(oldItem.inventorySlotType==InventorySlotType.MeleeWeapon || oldItem.inventorySlotType==InventorySlotType.RangedWeapon)
            {
                if (oldItem.inventorySlotType == InventorySlotType.MeleeWeapon)
                {
                    damageMelee.min = 0;
                    damageMelee.max = 0;
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
