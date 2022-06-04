using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab",
    order = 0)]
public class SpawnTargetPrefabEffect : EffectStrategy
{
    [SerializeField] Transform prefabToSpawn;
    [SerializeField] float destroyDelay = -1;
    [SerializeField] bool shouldSpawnOnPoint;
    [SerializeField] bool shouldSpawnOnHand;
    [SerializeField] bool staticSpawnPoint;
    [Tooltip("Input 'projectile damage' if you set 'isProjectile'")]
    [SerializeField] bool isProjectile;
    [SerializeField] float skillDamageMultiplier;
    GameObject player;

    Vector3 posX;


    public override void StartEffect(AbilityData data, Action finished)
    {
        data.StartCoroutine(Effect(data, finished));
    }

    private IEnumerator Effect(AbilityData data, Action finished)
    {

        // Transform instance = Instantiate(prefabToSpawn);
        Transform instance;
        if (shouldSpawnOnPoint)
        {
            instance = Instantiate(prefabToSpawn, data.GetUser().GetComponent<StateMachine>().GetHeadTransform());
            // instance.forward = data.GetUser().transform.forward;

        }

        else if (shouldSpawnOnHand)
        {
            if (isProjectile)
            {
                Transform target = data.GetUser().GetComponent<StateMachine>().Target;
                if (target != null)
                {
                   
                    instance = Instantiate(prefabToSpawn, data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform());
                    instance.GetComponent<TargetProjectile>().SetPosition(data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform().position);
                    Vector2 offset = data.GetUser().GetComponent<StateMachine>().GetUIOffset();
                    instance.GetComponent<TargetProjectile>().UpdateTarget(target, data.GetUser(), data.GetUser().GetComponent<BaseStats>().GetStat(Stat.Damage) * skillDamageMultiplier, (Vector3)offset);
                  
                }
            }
            if (staticSpawnPoint)
            {
                instance = Instantiate(prefabToSpawn);
                instance.position = data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform().position;
                instance.rotation = data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform().rotation;
            }
            else { instance = Instantiate(prefabToSpawn, data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform()); }

            // instance.position = data.GetUser().gameObject.transform.Find("arissa model").transform.Find("arissa:Hips").transform.Find("arissa:Spine").transform.Find("arissa:Spine1").transform.Find("arissa:Spine2").transform.Find("arissa:LeftShoulder").transform.Find("arissa:LeftArm").transform.Find("arissa:LeftForeArm").transform.Find("arissa:LeftHand").transform.position;
            // instance.position = data.GetUser().GetComponent<InstantiatePoints>().GetLeftHandTransform().position;
            //instance.forward = data.GetUser().transform.forward;
        }

        else
        {
            instance = Instantiate(prefabToSpawn, data.GetTargetedPoint(), Quaternion.identity);
            //instance.position = data.GetTargetedPoint();
        }

        if (destroyDelay > 0)
        {

            yield return new WaitForSeconds(destroyDelay);
            Destroy(instance.gameObject);
        }

        finished();
    }
}