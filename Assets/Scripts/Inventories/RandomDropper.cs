using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomDropper : ItemDropper
{
    [SerializeField] private float scatterDistance = 1;
    [SerializeField] private InventoryItem[] dropLibrary;
    [SerializeField] private int numerOfDrops = 2;

    private const int ATTEMPTS = 30;

    public void RandomDrop()
    {
        for (int i = 0; i < numerOfDrops; i++)
        {
            var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
            DropItem(item, 1);
        }
    }

    protected override Vector3 GetDropLocation()
    {
        for (int i = 0; i < ATTEMPTS; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return transform.position;
    }
}