using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Item"))]
public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
{
    //CONFIG DATA
    [SerializeField] string itemID = null;
    [SerializeField] string displayName = null;
    [SerializeField] [TextArea] string description = null;
    [SerializeField] Sprite icon = null;
    [SerializeField] private Pickup pickup = null;
    [SerializeField] bool stackable = false;

    //STATE
    static Dictionary<string, InventoryItem> itemLookupCache;

    public static InventoryItem GetFromID(string itemID)
    {
        if (itemLookupCache == null)
        {
            itemLookupCache = new Dictionary<string, InventoryItem>();
            var itemList = Resources.LoadAll<InventoryItem>("");
            foreach (var item in itemList)
            {
                if (itemLookupCache.ContainsKey(item.itemID))
                {
                    Debug.LogError(string.Format("Aynı öğeden birden fazla adet var"));
                    continue;
                }

                itemLookupCache[item.itemID] = item;
            }
        }

        if ((itemID == null) || !itemLookupCache.ContainsKey(itemID)) return null;
        return itemLookupCache[itemID];
    }

    public Pickup SpawnPickup(Vector3 position, int number, Transform parent)
    {
        var pickup = Instantiate(this.pickup, parent);
        pickup.transform.position = position;
        pickup.Setup(this, number);
        return pickup;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetItemID()
    {
        return itemID;
    }

    public bool IsStackable()
    {
        return stackable;
    }

    public string GetDisplayName()
    {
        return displayName;
    }

    public string GetDescription()
    {
        return description;
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if (string.IsNullOrWhiteSpace(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
    }
}