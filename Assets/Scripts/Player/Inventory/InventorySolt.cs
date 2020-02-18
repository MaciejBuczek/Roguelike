using UnityEngine;
using UnityEngine.UI;

public class InventorySolt : MonoBehaviour
{
    public Image icon;
    public Sprite defaultIcon = null;
    public Button slotButton;
    public GameObject itemDetails;
    public Item item;
    public bool isEquipmentSlot = false;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        slotButton.interactable = true;
    }
    public void RemoveItem()
    {
        item = null;
        icon.sprite = defaultIcon;
        if(!isEquipmentSlot)
            icon.enabled = false;
        slotButton.interactable = false;
    }
    public void AssignItemToSlot(InventorySolt inventorySlot)
    {
        if (inventorySlot.item)    
            SwapItems(inventorySlot);
        else
            MoveItem(inventorySlot);
    }
    public void SwapItems(InventorySolt inventorySlot)
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
    public void MoveItem(InventorySolt inventorySolt)
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
