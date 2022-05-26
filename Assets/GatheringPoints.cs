using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringPoints : MonoBehaviour
{

    Transform[] points;
    // Start is called before the first frame update
    void Start()
    {
        points = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            points[i] = transform.GetChild(i).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetPoints(int i)
    {
        print("here");
        return points[i];
    }
}
