using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equippable : Item
{
    public IntRange damage;
    public int armorModifier;
    public int critChance;
    public int speed;

    public override void Equip(InventorySlot inventorySlot)
    {
        base.Equip(inventorySlot);
        Equipment.instance.EquipItemFromSlot(inventorySlot);
    }
    public override void Unequip(InventorySlot inventorySlot)
    {
        base.Unequip(inventorySlot);
        Equipment.instance.UnequipItemFromSlot(inventorySlot);
    }
}

