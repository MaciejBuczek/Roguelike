using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item item;
    public Canvas canvas;
    public GameObject DragObject;
    public InventorySolt inventorySlot;
    private RectTransform rectTransform;
    private bool isDragging;
    [HideInInspector]
    public bool isDropped;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetComponent<InventorySolt>().slotButton.interactable)
        {
            Debug.Log("Begin Drag");
            item = GetComponent<InventorySolt>().item;

            DragObject.SetActive(true);
            DragObject.transform.position = transform.position;

            DragObject.GetComponent<Image>().sprite = item.icon;

            rectTransform = DragObject.GetComponent<RectTransform>();

            DragObject.GetComponent<CanvasGroup>().alpha = 0.6f;

            inventorySlot.RemoveItem();

            isDragging = true;
            isDropped = false;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isDragging)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isDragging)
        {        
            DragObject.GetComponent<Image>().sprite = null;
            if(!isDropped)
                inventorySlot.AddItem(item);      
            DragObject.SetActive(false);
            isDragging = false;
            Debug.Log("End Drag");
        }
    }
}
