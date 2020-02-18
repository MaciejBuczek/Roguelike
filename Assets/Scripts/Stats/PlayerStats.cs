public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        Equipment.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equippable newItem, Equippable oldItem)
    {
        if(newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
        }
    }
}
