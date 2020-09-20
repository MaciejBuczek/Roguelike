using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    #region Singleton
    public static Equipment Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("more then one instance of equipment found");
            return;
        }
        Instance = this;
    }
    #endregion

    public InventorySlot[] equipmentSlots;

    public delegate void OnEquipmentChanged(Equippable newItem, Equippable oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    
    public void EquipItemFromSlot(InventorySlot inventorySlot)
    {
        Equippable newItem, oldItem;
        InventorySlotType inventorySlotType = inventorySlot.item.inventorySlotType;
        oldItem = (Equippable)equipmentSlots[(int)inventorySlotType].item;
        newItem = (Equippable)inventorySlot.item;
        inventorySlot.AssignItemToSlot(equipmentSlots[(int)inventorySlotType]);
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
    }
    public void UnequipItemFromSlot(InventorySlot inventorySlot)
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
