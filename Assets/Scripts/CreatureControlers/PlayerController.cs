using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : AggressiveCreatureController
{
    #region Singleton
    public static PlayerController Instance;
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("More then one instance of player controller found");
        else
            Instance = this;
    }
    #endregion

    [HideInInspector] public int currentExp = 0, level = 1, nextLevelReq = 100;
    public Animator headAnimator, bodyAnimator;
    private readonly IntRange unarmedDamage = new IntRange(1, 5);
    private readonly int unarmedCritChance = 10, unarmedSpeed = 5;

    [HideInInspector] public Player player;
    private PlayerBars playerStatBars;

    protected override void Start()
    {
        Equipment.Instance.onEquipmentChanged += OnEquipmentChanged;
        player = (Player)creature;
        playerStatBars = (PlayerBars)statBars;
        playerStatBars.SetMaxExp(nextLevelReq);
        player.attackSpeedRanged = 0;
        if(Equipment.Instance.equipmentSlots[(int)InventorySlotType.MeleeWeapon].item == null)
        {
            player.damageMelee = unarmedDamage;
            player.critChance.SetBaseValue(unarmedCritChance);
            player.attackSpeedMelee = unarmedSpeed;
        }
        CalculateAll();
        base.Start();
    }
    public void ChangeMana(int amount)
    {
        currentMana -= amount;
        playerStatBars.SetMana(currentMana);
        
    }
    public void ChangeExp(int amount)
    {
        currentExp += amount;
        playerStatBars.SetExp(currentExp);
    }
    private void CalculateHealth()
    {
        player.health.SetBaseValue(player.strength.GetValue() * 2);
        currentHealth = player.health.GetValue();
        playerStatBars.SetMaxHealth(currentHealth);
    }
    private void CalculateDodge()
    {
        player.dodge.SetBaseValue(player.dexterity.GetValue());
    }
    private void CalculateMana()
    {
        player.mana.SetBaseValue(player.intelligence.GetValue() * 2);
        currentMana = player.mana.GetValue();
        playerStatBars.SetMaxMana(currentMana);
    }
    private void CalculateAll()
    {
        CalculateHealth();
        CalculateDodge();
        CalculateMana();
        PlayerStatsUI.Instance.ChangeStatistics();
    }
    void OnEquipmentChanged(Equippable newItem, Equippable oldItem)
    {
        if (newItem != null)
        {
            if (newItem.armorModifier != 0)
                player.armor.AddModifier(newItem.armorModifier);
            else if (newItem.inventorySlotType == InventorySlotType.MeleeWeapon || newItem.inventorySlotType == InventorySlotType.RangedWeapon)
            {
                if (newItem.inventorySlotType == InventorySlotType.MeleeWeapon)
                {
                    player.damageMelee = newItem.damage;
                    player.critChance.SetBaseValue(newItem.critChance);
                }
                else
                {
                    player.damageRanged = newItem.damage;
                }
            }
        }
        else
        {
            if (oldItem.armorModifier != 0)
                player.armor.RemoveModifier(oldItem.armorModifier);
            else if (oldItem.inventorySlotType == InventorySlotType.MeleeWeapon || oldItem.inventorySlotType == InventorySlotType.RangedWeapon)
            {
                if (oldItem.inventorySlotType == InventorySlotType.MeleeWeapon)
                {
                    player.damageMelee = unarmedDamage;
                    player.critChance.SetBaseValue(unarmedCritChance);
                    player.attackSpeedMelee = unarmedSpeed;
                }
                else
                {
                    player.damageRanged.min = 0;
                    player.damageRanged.max = 0;
                }
            }
        }
        PlayerStatsUI.Instance.ChangeStatistics();
    }
    public override void ApplyAnimator()
    {
        headAnimator.runtimeAnimatorController = player.headAnimatorOverride;
        bodyAnimator.runtimeAnimatorController = player.mainAnimatorOverride;
    }
    public int GetCurrentExp()
    {
        return currentExp;
    }

}
