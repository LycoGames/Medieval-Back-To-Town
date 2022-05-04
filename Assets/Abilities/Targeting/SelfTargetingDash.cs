using System;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Self Targeting Dash", menuName = "Abilities/Targeting/SelfDash", order = 0)]
public class SelfTargetingDash : TargetingStrategy
{
    public float tpDistance;
    public float speed;
    
    public override void StartTargeting(AbilityData data, Action finished)
    {
        data.SetTargets(new GameObject[] { data.GetUser() });
        data.SetTargetedPoint(data.GetUser().transform.position);
        CustomTeleport(data, finished);
        finished();
    }

    void CustomTeleport(AbilityData data, Action finished)
    {
        Vector3 tpPoint = data.GetUser().transform.position + data.GetUser().transform.forward * tpDistance;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(tpPoint, out hit, 10f, NavMesh.AllAreas))
        {
            data.GetUser().transform.position = Vector3.MoveTowards(data.GetUser().transform.position, hit.position, Time.deltaTime * speed);
        }
    }
}
