using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable, IPredicateEvaluator
{
    //CONFIG DATA
    [SerializeField] int inventorySize = 16;

    //STATE

    InventorySlot[] slots;
    public event Action inventoryUpdated;

    public struct InventorySlot
    {
        public InventoryItem item;
        public int number;
    }

    [Serializable]
    private struct InventorySlotRecord
    {
        public string itemID;
        public int number;
    }


    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
    }

    public static Inventory GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<Inventory>();
    }

    // Gelen eşyaya envanterde yer var mı?
    public bool HasSpaceFor(InventoryItem item)
    {
        return FindSlot(item) >= 0;
    }

    public int GetSize()
    {
        return slots.Length;
    }

    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots[i].item = item;
        slots[i].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }

        GetComponent<QuestList>().UpdateCollectObjectiveStatus(item, number);
        return true;
    }

    public bool HasItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return true;
            }
        }

        return false;
    }

    public InventoryItem GetItemInSlot(int slot)
    {
        return slots[slot].item;
    }

    public int GetNumberInSlot(int index)
    {
        return slots[index].number;
    }

    public int GetSlot(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }

        return -1;
    }

    public void RemoveFromSlot(int slot, int number)
    {
        slots[slot].number -= number;
        if (slots[slot].number <= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }

        //TODO
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    public bool AddItemToSlot(int slot, InventoryItem item, int number)
    {
        if (slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number);
        }

        var i = FindStack(item);

        if (i >= 0)
        {
            slot = i;
        }

        slots[slot].item = item;
        slots[slot].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }

        GetComponent<QuestList>().UpdateCollectObjectiveStatus(item, number);

        return true;
    }


    public int FindSlot(InventoryItem item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            i = FindEmptySlot();
        }

        return i;
    }

    public int FindStack(InventoryItem item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }

        return -1;
    }

    public void DeleteItem(InventoryItem item, int number)
    {
        int slotNumberOfStack = FindStack(item);
        if (slots[slotNumberOfStack].number == number)
        {
            slots[slotNumberOfStack].number = 0;
            slots[slotNumberOfStack].item = null;
        }
        else if (slots[slotNumberOfStack].number > number)
        {
            slots[slotNumberOfStack].number -= number;
        }

        if (inventoryUpdated != null)
            inventoryUpdated();
    }

    public object CaptureState()
    {
        var slotRecords = new InventorySlotRecord[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (slots[i].item != null)
            {
                slotRecords[i].itemID = slots[i].item.GetItemID();
                slotRecords[i].number = slots[i].number;
            }
        }

        return slotRecords;
    }

    public void RestoreState(object state)
    {
        var slotRecords = (InventorySlotRecord[]) state;

        for (int i = 0; i < inventorySize; i++)
        {
            slots[i].item = InventoryItem.GetFromID(slotRecords[i].itemID);
            slots[i].number = slotRecords[i].number;
        }

        if (inventoryUpdated != null)
            inventoryUpdated();
    }

    public bool? Evaluate(string predicate, string[] parameters)
    {
        switch (predicate)
        {
            case "HasInventoryItem":
                return HasItem(InventoryItem.GetFromID(parameters[0]));
        }

        return null;
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return i;
            }
        }

        return -1;
    }
}