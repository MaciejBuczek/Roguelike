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
    public void OnSlotButton()
    {
        itemDetails.SetActive(true);
        itemDetails.GetComponent<ItemDetails>().DisplayItemDetails(this);
    }
}
