using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBetweenPointsWorker : Worker
{
    [SerializeField] private TravelPoints[] travelPoints;

    [Serializable]
    private struct TravelPoints
    {
        private Vector3 standPoint;
        private Vector3 posToLook;
    }

    void Start()
    {
    }

    void Update()
    {
    }
}