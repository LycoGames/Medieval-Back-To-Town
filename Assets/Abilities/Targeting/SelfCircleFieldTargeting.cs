using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Self Circle Field Targeting", menuName = "Abilities/Targeting/CircleField", order = 0)]
public class SelfCircleFieldTargeting : TargetingStrategy
{
    [SerializeField] private int circleRadius;

    public override void StartTargeting(AbilityData data, Action finished)
    {
        data.SetTargets(new GameObject[] { data.GetUser() });
        data.SetTargetedPoint(data.GetUser().transform.position);
        data.StartCoroutine(Targeting(data, finished));
        finished();
    }

    private IEnumerator Targeting(AbilityData data, Action finished)
    {
        data.SetTargets(GetGameObjectsInRadius(data.GetUser().transform.position));
        yield return null;
    }

    private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
    {
        RaycastHit[] hits = Physics.SphereCastAll(point, circleRadius, Vector3.up, 0);
        foreach (var hit in hits)
        {
            yield return hit.collider.gameObject;
        }
    }
}