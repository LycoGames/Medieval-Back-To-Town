using RPG.Saving;
using UnityEngine;


/// <summary>
/// Spawns pickups that should exist on first load in a level. This
/// automatically spawns the correct prefab for a given inventory item.
/// </summary>
public class PickupSpawner : MonoBehaviour, ISaveable
{
    // CONFIG DATA
    [SerializeField] InventoryItem item = null;

    [SerializeField] private int number = 1;
    //TODO

    private Transform pickupParent;

    // LIFECYCLE METHODS
    private void Awake()
    {
        // Spawn in Awake so can be destroyed by save system after.
        pickupParent = GameObject.FindGameObjectWithTag("PickupParent").transform;
        SpawnPickup();
    }

    // PUBLIC

    /// <summary>
    /// Returns the pickup spawned by this class if it exists.
    /// </summary>
    /// <returns>Returns null if the pickup has been collected.</returns>
    public Pickup GetPickup()
    {
        return GetComponentInChildren<Pickup>();
    }

    /// <summary>
    /// True if the pickup was collected.
    /// </summary>
    public bool isCollected()
    {
        return GetPickup() == null;
    }

    //PRIVATE

    private void SpawnPickup()
    {
        //TODO
        var spawnedPickup = item.SpawnPickup(transform.position, number, pickupParent);
        spawnedPickup.transform.SetParent(transform);
    }

    private void DestroyPickup()
    {
        if (GetPickup())
        {
            Destroy(GetPickup().gameObject);
        }
    }

    public object CaptureState()
    {
        return isCollected();
    }

    public void RestoreState(object state)
    {
        bool shouldBeCollected = (bool) state;

        if (shouldBeCollected && !isCollected())
        {
            DestroyPickup();
        }

        if (!shouldBeCollected && isCollected())
        {
            SpawnPickup();
        }
    }
}