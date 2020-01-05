using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("more then one instance of inventory found");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChange();
    public OnItemChange OnItemChangeCallBack;

    public List<Item> items = new List<Item>();

    public int maxSpace = 20;

    public bool Add(Item item)
    {
        if (items.Count < maxSpace)
        {
            items.Add(item);
            if(OnItemChangeCallBack!=null)
                OnItemChangeCallBack.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Inventory Full");
            return false;
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
