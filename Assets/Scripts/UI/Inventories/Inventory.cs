using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable
{
    //CONFIG DATA
    [SerializeField] int inventorySize = 16;

    //STATE

    InventoryItem[] slots;

    public event Action inventoryUpdated;


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

    public bool AddToFirstEmptySlot(InventoryItem item)
    {
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots[i] = item;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    public bool HasItem(InventoryItem item)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i], item))
            {
                return true;
            }
        }
        return false;
    }

    public InventoryItem GetItemInSlot(int slot)
    {
        return slots[slot];
    }

    public void RemoveFromSlot(int slot)
    {

        slots[slot] = null;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    public bool AddItemToSlot(int slot, InventoryItem item)
    {
        if (slots[slot] != null)
        {
            return AddToFirstEmptySlot(item);
        }

        slots[slot] = item;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    private void Awake()
    {
        slots = new InventoryItem[inventorySize];
        slots[0] = InventoryItem.GetFromID("18b323ff-e566-40e5-9abd-76a3632283f7");
        slots[1] = InventoryItem.GetFromID("05008513-e556-4b50-8b3d-26f498785624");
    }

    private int FindSlot(InventoryItem item)
    {
        return FindEmptySlot();
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public object CaptureState()
    {
        var slotStrings = new string[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (slots[i] != null)
            {
                slotStrings[i] = slots[i].GetItemID();
            }
        }
        return slotStrings;
    }

    public void RestoreState(object state)
    {
        var slotStrings = (string[])state;

        for (int i = 0; i < inventorySize; i++)
        {
            slots[i] = InventoryItem.GetFromID(slotStrings[i]);
        }

        if (inventoryUpdated != null)
            inventoryUpdated();
    }
}
