using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDetails : MonoBehaviour
{
    InventorySolt inventorySolt;
    public Image image;
    public Text itemName;
    public Text itemDescription;
    public Text itemInteractionButton;

    // Start is called before the first frame update
    private void Awake()
    {
        inventorySolt = null;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (IsMouseOutsideActivePanel())
            {
                gameObject.SetActive(false);
            }
            return;
        }
    }
    bool IsMouseOutsideActivePanel()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        for(int i=0; i<raycastResults.Count; i++)
        {
            if (!raycastResults[i].gameObject.CompareTag("ActivePanel"))
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count == 0;

    }
    public void DisplayItemDetails(InventorySolt newInventorySlot)
    {
        inventorySolt = newInventorySlot;
        itemName.text = inventorySolt.item.name;
        itemDescription.text = inventorySolt.item.description;
        image.sprite = inventorySolt.item.icon;
        if (inventorySolt.item is Equippable)
        {
            if(inventorySolt.isEquipmentSlot)
                itemInteractionButton.text = "Unequip";
            else
                itemInteractionButton.text = "Equip";
        }
        else
            itemInteractionButton.text = "Use";
    }
    public void UseValidFunction()
    {
        if(inventorySolt.item is Equippable)
        {
            if (inventorySolt.isEquipmentSlot)
            {
                inventorySolt.item.Unequip(inventorySolt);
            }else
                inventorySolt.item.Equip(inventorySolt);
        }
        gameObject.SetActive(false);
    }
    public void UseItem()
    {
        inventorySolt.item.Use();
    }
    public void EquipItem()
    {
        inventorySolt.item.Equip(inventorySolt);
    }
    public void DropItem()
    {
        inventorySolt.item.Drop();
    }
    public void ThrowItem()
    {
        inventorySolt.item.Throw();
    }
}
