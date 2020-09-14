using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventorySlotType inventorySlotType;
    public InventorySlot inventorySlot;
    private ItemDragHandler itemDragHandler;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (inventorySlotType != InventorySlotType.Any && itemDragHandler.item.inventorySlotType != inventorySlotType)
                return;
            if (inventorySlot.isEquipmentSlot)
                Equipment.instance.EquipItemFromSlot(itemDragHandler.inventorySlot);
            else if (itemDragHandler.inventorySlot.isEquipmentSlot)
                Equipment.instance.UnequipItemFromSlot(itemDragHandler.inventorySlot);
            else if (inventorySlot.item == null)
                itemDragHandler.inventorySlot.MoveItem(inventorySlot);
            else
                itemDragHandler.inventorySlot.SwapItems(inventorySlot);
            itemDragHandler.isDropped = true;
        }
    }
}
