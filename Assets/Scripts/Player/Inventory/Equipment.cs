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
    
    public void EquipItemFromSlot(InventorySolt inventorySlot)
    {
        Equippable newItem, oldItem;
        InventorySlotType inventorySlotType = inventorySlot.item.inventorySlotType;
        inventorySlot.AssignItemToSlot(equipmentSlots[(int)inventorySlotType]);
        newItem = (Equippable)equipmentSlots[(int)inventorySlotType].item;
        oldItem = (Equippable)inventorySlot.item;
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
    }
    public void UnequipItemFromSlot(InventorySolt inventorySlot)
    {
        Equippable newItem, oldItem;
        InventorySlotType inventorySlotType = inventorySlot.item.inventorySlotType;
        oldItem = (Equippable)inventorySlot.item;
        Inventory.instance.Add(inventorySlot.item);
        inventorySlot.RemoveItem();
        newItem = (Equippable)equipmentSlots[(int)inventorySlotType].item;
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
    }
}
