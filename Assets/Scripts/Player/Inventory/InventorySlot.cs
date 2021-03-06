﻿using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Image background;
    public Sprite defaultIcon = null;
    public Button slotButton;
    public GameObject itemDetails;
    public Item item;
    public bool isEquipmentSlot = false;
    public Sprite[] backgrounds;

    private enum Backrounds {None, Common, Uncommon, Rare, Legendary};

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        slotButton.interactable = true;
        background.sprite = backgrounds[(int)item.rarity + 1];
    }
    public void RemoveItem()
    {
        item = null;
        icon.sprite = defaultIcon;
        background.sprite = backgrounds[0];
        if(!isEquipmentSlot)
            icon.enabled = false;
        slotButton.interactable = false;
    }
    public void AssignItemToSlot(InventorySlot inventorySlot)
    {
        if (inventorySlot.item)    
            SwapItems(inventorySlot);
        else
            MoveItem(inventorySlot);
    }
    public void SwapItems(InventorySlot inventorySlot)
    {
        if (isEquipmentSlot)
        {
            Inventory.instance.items.Remove(inventorySlot.item);
            Inventory.instance.items.Add(item);
            //Equipment.instance.EquipItemFromSlot(inventorySlot);
        }
        else if (inventorySlot.isEquipmentSlot)
        {
            Inventory.instance.items.Remove(item);
            Inventory.instance.items.Add(inventorySlot.item);
            //Equipment.instance.EquipItemFromSlot(inventorySlot);
        }
        Item tempItem = item;
        AddItem(inventorySlot.item);
        inventorySlot.AddItem(tempItem);
    }
    public void MoveItem(InventorySlot inventorySolt)
    {
        if (isEquipmentSlot)
            Inventory.instance.items.Add(item);
        else if (inventorySolt.isEquipmentSlot)
            Inventory.instance.items.Remove(item);
        inventorySolt.AddItem(item);
        RemoveItem();
    }
    public void OnSlotButton()
    {
        itemDetails.SetActive(true);
        itemDetails.GetComponent<ItemDetails>().DisplayItemDetails(this);
    }
}
