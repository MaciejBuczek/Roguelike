using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventorySolt currentSlot;
    private ItemDragHandler itemDragHandler;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            itemDragHandler.isDropped = true;
            if(currentSlot.item == null)
            {
                if (currentSlot.isEquipmentSlot)
                    Inventory.instance.Remove(itemDragHandler.item);
                if(itemDragHandler.inventorySlot.isEquipmentSlot)
                    Inventory.instance.items.Add(itemDragHandler.item);
            }
            else
            {
                if (currentSlot.isEquipmentSlot)
                {
                    itemDragHandler.inventorySlot.AddItem(currentSlot.item);
                    Inventory.instance.items.Add(currentSlot.item);
                    Inventory.instance.Remove(itemDragHandler.item);
                }
                else if(itemDragHandler.inventorySlot.isEquipmentSlot)
                {
                    itemDragHandler.inventorySlot.AddItem(currentSlot.item);
                    Inventory.instance.items.Add(itemDragHandler.item);
                    Inventory.instance.Remove(currentSlot.item);
                }
                else
                {
                    itemDragHandler.inventorySlot.AddItem(currentSlot.item);
                    currentSlot.AddItem(itemDragHandler.item);
                }
            }
            currentSlot.AddItem(itemDragHandler.item);
        }
    }
}
