using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equippable : Item
{
    public int damageModifier;
    public int armorModifier;
    public int critChance;

    public override void Equip(InventorySolt inventorySlot)
    {
        base.Equip(inventorySlot);
        Equipment.instance.EquipItem(inventorySlot);
    }
    public override void Unequip(InventorySolt inventorySlot)
    {
        base.Unequip(inventorySlot);
        Equipment.instance.Unequip(inventorySlot);
    }
}

