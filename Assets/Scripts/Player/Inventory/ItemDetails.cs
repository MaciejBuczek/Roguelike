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
                Debug.Log("ugabuga");
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
            if (raycastResults[i].gameObject.tag != "ActivePanel")
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
    }
    public void UseItem()
    {
        inventorySolt.item.Use();
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
