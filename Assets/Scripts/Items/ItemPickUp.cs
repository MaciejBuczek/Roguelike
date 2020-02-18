using UnityEngine;

public class ItemPickUp : Interactable
{

    public Item item;

    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    public override void SetSprite()
    {
        Debug.Log("b");
        this.GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        if (Inventory.instance.Add(item)) 
            Destroy(gameObject);
    }
}
