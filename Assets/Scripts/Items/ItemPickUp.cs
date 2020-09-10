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
        this.GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    void PickUp()
    {
        if (Inventory.instance.Add(item)) 
            Destroy(gameObject);
    }
}
