using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Events;

public class Equipment : MonoBehaviour, ISaveable
{
    // STATE
    Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();
    public UnityEvent updateEquipmentUiStatsEvent;

    // PUBLIC

    public event Action equipmentUpdated;
    private ICommonFunctions commonFunctions;

    private void Start()
    {
        commonFunctions = GetComponent<ICommonFunctions>();
    }

    public EquipableItem GetItemInSlot(EquipLocation equipLocation)
    {
        return !equippedItems.ContainsKey(equipLocation) ? null : equippedItems[equipLocation];
    }

    public void AddItem(EquipLocation slot, EquipableItem item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems[slot] = item;
        if (TryGetComponent(out WStateMachine stateMachine) && slot == EquipLocation.PrimaryWeapon)
        {
            GetComponent<WarriorFighter>().EquipWeapon(item as WeaponConfig);
        }

        UpdateHudsAndStats(item);

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void RemoveItem(EquipLocation slot)
    {
        EquipableItem item = GetItemInSlot(slot);
        equippedItems.Remove(slot);
        UpdateHudsAndStats(item);

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    private void UpdateHudsAndStats(EquipableItem item)
    {
        StatsEquipableItem stats = item as StatsEquipableItem;
        if (stats != null)
        {
            if (stats.GetAdditiveModifiers(Stat.Damage).Any() ||
                stats.GetPercentageModifiers(Stat.Damage).Any())
            {
                updateEquipmentUiStatsEvent?.Invoke();
            }

            if (stats.GetAdditiveModifiers(Stat.Defence).Any() ||
                stats.GetPercentageModifiers(Stat.Defence).Any())
            {
                updateEquipmentUiStatsEvent?.Invoke();
            }

            if (stats.GetAdditiveModifiers(Stat.CriticChance).Any() ||
                stats.GetPercentageModifiers(Stat.CriticChance).Any())
            {
                updateEquipmentUiStatsEvent?.Invoke();
            }

            if (stats.GetAdditiveModifiers(Stat.Accuracy).Any() ||
                stats.GetPercentageModifiers(Stat.Accuracy).Any())
            {
                updateEquipmentUiStatsEvent?.Invoke();
            }

            if (stats.GetAdditiveModifiers(Stat.MovementSpeed).Any() ||
                stats.GetPercentageModifiers(Stat.MovementSpeed).Any())
            {
                commonFunctions.UpdateModifiedSpeed();
            }

            if (stats.GetAdditiveModifiers(Stat.AbilityPower).Any() ||
                stats.GetPercentageModifiers(Stat.AbilityPower).Any())
            {
                commonFunctions.UpdateAdditiveAbilityDamage();
            }

            if (stats.GetAdditiveModifiers(Stat.Health).Any() || stats.GetPercentageModifiers(Stat.Health).Any())
            {
                GetComponent<Health>().SetNewMaxHealthOnHUD();
            }


            if (stats.GetAdditiveModifiers(Stat.Mana).Any() || stats.GetPercentageModifiers(Stat.Mana).Any())
                GetComponent<Mana>().SetNewMaxMana();
        }
    }

    public IEnumerable<EquipLocation> GetAllPopulatedSlots()
    {
        return equippedItems.Keys;
    }


    // PRIVATE

    object ISaveable.CaptureState()
    {
        var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
        foreach (var pair in equippedItems)
        {
            equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
        }

        return equippedItemsForSerialization;
    }

    void ISaveable.RestoreState(object state)
    {
        equippedItems = new Dictionary<EquipLocation, EquipableItem>();

        var equippedItemsForSerialization = (Dictionary<EquipLocation, string>) state;

        foreach (var pair in equippedItemsForSerialization)
        {
            var item = (EquipableItem) InventoryItem.GetFromID(pair.Value);
            if (item != null)
            {
                equippedItems[pair.Key] = item;
            }
        }
    }
}