using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "Uknown Item";
    public Sprite icon = null;
    public string description = "Unknow Item";

    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
    public virtual void Drop()
    {
        Debug.Log("Dropping " + name);
    }
    public virtual void Throw()
    {
        Debug.Log("Throwing " + name);
    }
}
