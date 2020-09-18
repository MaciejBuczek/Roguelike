using UnityEngine;

public class ItemPickUp : Interactable
{

    public Item item;
    private void Start()
    {
        transform.name = item.name;
    }

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
