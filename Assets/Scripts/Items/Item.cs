using UnityEngine;

public enum InventorySlotType { MeleeWeapon, RangedWeapon, AbilityItem, Armor, Ring, Trinket, Any };

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "Uknown Item";
    public Sprite icon = null;
    public string description = "Unknow Item";
    public InventorySlotType inventorySlotType;

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
    public virtual void Equip(InventorySolt inventorySlot)
    {
        Debug.Log("Equipping " + name);
    }
    public virtual void Unequip(InventorySolt inventorySlot)
    {
        Debug.Log("Unequip" + name);
    }
    public virtual void Drop()
    {
        Debug.Log("Dropping " + name);
    }
    public virtual void Throw()
    {
        Debug.Log("Throwing " + name);
    }
}
