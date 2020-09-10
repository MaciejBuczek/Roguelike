using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemSoltsParent;
    public GameObject inventoryUI;
    public bool isInventoryOpen = false;
    Inventory inventory;
    InventorySolt[] slots;

    #region Singleton
    public static InventoryUI Instance;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("More then one instance of Inventory UI found");
        else
            Instance = this;
    }
    #endregion

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
            isInventoryOpen = !isInventoryOpen;
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
