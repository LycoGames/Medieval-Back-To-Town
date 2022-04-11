using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemDropper : MonoBehaviour, ISaveable
{
    private List<Pickup> droppedItems = new List<Pickup>();
    private List<DropRecord> otherSceneDroppedItems = new List<DropRecord>();


    public void DropItem(InventoryItem item, int number)
    {
        SpawnPickup(item, number, GetDropLocation());
    }

    protected virtual Vector3 GetDropLocation()
    {
        return transform.position;
    }

    public void SpawnPickup(InventoryItem item, int number, Vector3 spawnLocation)
    {
        var pickup = item.SpawnPickup(spawnLocation, number);
        droppedItems.Add(pickup);
    }

    [Serializable]
    private struct DropRecord
    {
        public string itemID;
        public SerializableVector3 position;
        public int number;
        public int sceneBuildIndex;
    }

    public object CaptureState()
    {
        RemoveDestroyedDrops();
        var droppedItemList = new List<DropRecord>();
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        foreach (Pickup pickup in droppedItems)
        {
            var droppedItem = new DropRecord();
            droppedItem.itemID = pickup.GetItem().GetItemID();
            droppedItem.position = new SerializableVector3(pickup.transform.position);
            droppedItem.number = pickup.GetNumber();
            droppedItem.sceneBuildIndex = buildIndex;
            droppedItemList.Add(droppedItem);
        }

        droppedItemList.AddRange(otherSceneDroppedItems);
        return droppedItemList;
    }

    public void RestoreState(object state)
    {
        var droppedItemList = (List<DropRecord>) state;
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        otherSceneDroppedItems.Clear();
        foreach (var item in droppedItemList)
        {
            if (item.sceneBuildIndex != buildIndex)
            {
                otherSceneDroppedItems.Add(item);
                continue;
            }

            var pickupItem = InventoryItem.GetFromID(item.itemID);
            Vector3 position = item.position.ToVector();
            int number = item.number;
            SpawnPickup(pickupItem, number, position);
        }
    }

    private void RemoveDestroyedDrops()
    {
        var newList = new List<Pickup>();
        foreach (var item in droppedItems)
        {
            if (item != null)
            {
                newList.Add(item);
            }
        }

        droppedItems = newList;
    }
}