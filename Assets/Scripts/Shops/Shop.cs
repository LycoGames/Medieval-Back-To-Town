using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{

    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantintyInTransaction;
    }

    public event Action onChange;

    public IEnumerable<ShopItem> GetFilteredItems() { return null; }
    public void SelectFilter(ItemCategory category) { }
    public ItemCategory GetFilter() { return ItemCategory.None; }
    public void SelectMode(bool isBuying) { }
    public bool isBuyingMode() { return true; }
    public bool CanTransact() { return true; }
    public void ConfirmTranstaction() { }
    public float TransactionTotal() { return 0; }
    public void AddToTransaction(InventoryItem item, int quantity) { }
}
