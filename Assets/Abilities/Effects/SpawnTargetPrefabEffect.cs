using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab", order = 0)]
public class SpawnTargetPrefabEffect : EffectStrategy
{
    [SerializeField] Transform prefabToSpawn;
    [SerializeField] float destroyDelay = -1;
    [SerializeField] bool shouldSpawnOnPoint;


    public override void StartEffect(AbilityData data, Action finished)
    {
        data.StartCoroutine(Effect(data, finished));
    }

    private IEnumerator Effect(AbilityData data, Action finished)
    {
        Transform instance = Instantiate(prefabToSpawn);
        if (shouldSpawnOnPoint)
        {
            instance.position = GameObject.Find("Bow pivot").transform.position;
            instance.forward = data.GetUser().transform.up;
        }
        else
        {
            instance.position = data.GetTargetedPoint();
        }
        if (destroyDelay > 0)
        {
            yield return new WaitForSeconds(destroyDelay);
            Destroy(instance.gameObject);
        }
        finished();
    }
}