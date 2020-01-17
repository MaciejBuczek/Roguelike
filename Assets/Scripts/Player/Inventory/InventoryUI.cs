using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemSoltsParent;
    public GameObject inventoryUI;
    Inventory inventory;
    InventorySolt[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.OnItemChangeCallBack += UpdateUI;
        slots = itemSoltsParent.GetComponentsInChildren<InventorySolt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        Debug.Log("Update UI");
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(inventory.items[inventory.items.Count - 1]);
                break;
            }
        }
    }
}
