using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

public class Equipment : MonoBehaviour, ISaveable
{
    // STATE
    Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

    // PUBLIC

    public event Action equipmentUpdated;

    public EquipableItem GetItemInSlot(EquipLocation equipLocation)
    {
        if (!equippedItems.ContainsKey(equipLocation))
        {
            return null;
        }

        return equippedItems[equipLocation];
    }

    public void AddItem(EquipLocation slot, EquipableItem item)
    {
        Debug.Assert(item.GetAllowedEquipLocation() == slot);

        equippedItems[slot] = item;

        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
    }

    public void RemoveItem(EquipLocation slot)
    {
        equippedItems.Remove(slot);
        if (equipmentUpdated != null)
        {
            equipmentUpdated();
        }
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

        var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

        foreach (var pair in equippedItemsForSerialization)
        {
            var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
            if (item != null)
            {
                equippedItems[pair.Key] = item;
            }
        }
    }

}

