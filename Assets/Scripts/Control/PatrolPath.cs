using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    const float waypointGizmoRadius = 0.3f;
    [Range(0, 10)] [SerializeField] int PathhID;

    private List<GameObject> returningPathFollowers;
    private List<GameObject> returningRPathFollowers;

    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.color = Color.magenta; //ilk child'a renk atmıyordu en üste taşıdım.
            Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
        }
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }

    public int GetWaypointIndex(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (GetWaypoint(i) == transform.position)
            {
                return i;
            }
        }

        return 0;
    }

    public int GetNextIndex(int i)
    {
        // başlangıc point'inden sonuncuya cizgi cekmiyordu. 
        //Bu yüzden en son gelen i'nin 1 fazlası toplam child'e eşitse 0 yani 1. pointi yolladım.
        if (i + 1 == transform.childCount)
        {
            return 0;
        }

        return i + 1;
    }

    public int GetReverseNextIndex(int i)
    {
        return i - 1 < 0 ? transform.childCount - 1 : i - 1;
    }

    public int GetPathID()
    {
        return PathhID;
    }

    public int GetClosestIndex(Vector3 position)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < transform.childCount; i++)
        {
            float distance = Vector3.Distance(position, transform.GetChild(i).position);
            if (distance > closestDistance) continue;
            closestDistance = distance;
            closestIndex = i;
        }

        return closestIndex;
    }

    public int GetRoundTripNextIndex(int currentWaypointIndex, GameObject pathFollower)
    {
        returningPathFollowers ??= new List<GameObject>();
        if (!returningPathFollowers.Contains(pathFollower))
        {
            if (currentWaypointIndex == transform.childCount - 1)
            {
                returningPathFollowers.Add(pathFollower);
                return GetReverseNextIndex(currentWaypointIndex);
            }

            return GetNextIndex(currentWaypointIndex);
        }

        if (currentWaypointIndex == 0)
        {
            returningPathFollowers.Remove(pathFollower);
            return GetNextIndex(currentWaypointIndex);
        }

        return GetReverseNextIndex(currentWaypointIndex);
    }

    public int GetReverseRoundTripNextIndex(int currentWaypointIndex, GameObject pathFollower)
    {
        returningRPathFollowers ??= new List<GameObject>();
        if (!returningRPathFollowers.Contains(pathFollower))
        {
            if (currentWaypointIndex == 0)
            {
                returningRPathFollowers.Add(pathFollower);
                return GetNextIndex(currentWaypointIndex);
            }

            return GetReverseNextIndex(currentWaypointIndex);
        }

        if (currentWaypointIndex == transform.childCount - 1)
        {
            returningRPathFollowers.Remove(pathFollower);
            return GetReverseNextIndex(currentWaypointIndex);
        }

        return GetNextIndex(currentWaypointIndex);
    }
}