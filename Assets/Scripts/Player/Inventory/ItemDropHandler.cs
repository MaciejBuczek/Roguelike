using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventorySlotType equipmentSlot;
    public InventorySolt inventorySlot;
    private ItemDragHandler itemDragHandler;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("bazinga");
        if (eventData.pointerDrag != null)
        {
            itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
            if (inventorySlot.item == null)
                itemDragHandler.inventorySlot.MoveItem(inventorySlot);
            else
                itemDragHandler.inventorySlot.SwapItems(inventorySlot);
            itemDragHandler.isDropped = true;
        }
    }
}
