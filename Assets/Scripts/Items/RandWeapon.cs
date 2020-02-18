using UnityEngine;

public class RandWeapon : MonoBehaviour
{

    public Equippable[] Tier1;
    public Equippable[] Tier2;
    public Equippable[] Tier3;

    public ItemPickUp ItemPickUp;

    private Equippable item;

    // Start is called before the first frame update
    void Start()
    {
        int tier = Random.Range(1, 4);
        if (tier == 1)
        {
            item = Tier1[Random.Range(0, Tier1.Length)];
        }
        else if (tier == 2)
        {
            item = Tier2[Random.Range(0, Tier2.Length)];
        }
        else if (tier == 3)
        {
            item = Tier3[Random.Range(0, Tier3.Length)];
        }
        ItemPickUp.item = item;
    }
}
