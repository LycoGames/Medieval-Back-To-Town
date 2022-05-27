using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringPoints : MonoBehaviour
{

    const float waypointGizmoRadius = 0.3f;

    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.color = Color.green; //ilk child'a renk atmıyordu en üste taşıdım.
            Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
        }
    }

    public Vector3 GetWaypoint(int i)
    {
        if (i > transform.childCount - 1) return Vector3.zero;
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


}
