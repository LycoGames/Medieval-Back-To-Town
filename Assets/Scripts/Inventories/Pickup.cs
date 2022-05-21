using UnityEngine;

/// <summary>
/// To be placed at the root of a Pickup prefab. Contains the data about the
/// pickup such as the type of item and the number.
/// </summary>
public class Pickup : MonoBehaviour
{
    InventoryItem item;
    private int number;

    Inventory inventory;


    private void Awake()
    {


    }

    // PUBLIC
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }

    public void Setup(InventoryItem item, int number)
    {
        this.item = item;
        this.number = number;
    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public int GetNumber()
    {
        return number;
    }

    public void PickupItem()
    {
        //TODO
        bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
        if (foundSlot)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBePickedUp()
    {
        return inventory.HasSpaceFor(item);
    }
}