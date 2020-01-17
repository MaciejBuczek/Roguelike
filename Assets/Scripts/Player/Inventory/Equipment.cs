using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    #region Singleton
    public static Equipment instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more then one instance of equipment found");
            return;
        }
        instance = this;
    }
    #endregion

    public InventorySolt[] equipmentSlots;

    public delegate void OnEquipmentChanged(Equippable newItem, Equippable oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    
    public void EquipItem(InventorySolt inventorySlot)
    {
        inventorySlot.AssignItemToSlot(equipmentSlots[(int)inventorySlot.item.inventorySlotType]);
        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke((Equippable)inventorySlot.item, (Equippable)equipmentSlots[(int)inventorySlot.item.inventorySlotType].item);
        }
    }
    public void Unequip(InventorySolt inventorySlot)
    {
        Inventory.instance.Add(inventorySlot.item);
        inventorySlot.RemoveItem();
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke((Equippable)inventorySlot.item, (Equippable)equipmentSlots[(int)inventorySlot.item.inventorySlotType].item);
        }
    }
}
