using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDropper : ItemDropper
{
    [SerializeField] private float scatterDistance = 1.5f;

    private const int ATTEMPTS = 30;
    protected override Vector3 GetDropLocation()
    {
        for (int i = 0; i < ATTEMPTS; i++)
        {
            Vector3 randomPoint = transform.position + Random.onUnitSphere * scatterDistance;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }
}
