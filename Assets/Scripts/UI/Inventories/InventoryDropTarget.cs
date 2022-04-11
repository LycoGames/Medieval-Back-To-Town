using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDropTarget : MonoBehaviour,IDragDestination<InventoryItem>
{
    public int MaxAcceptable(InventoryItem item)
    {
        return int.MaxValue;
    }

    public void AddItems(InventoryItem item, int number)
    {
        //TODO MULTIPLAYER
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ItemDropper>().DropItem(item,number);
    }
}
