using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Self Prefab Effect", menuName = "Abilities/Effects/Spawn Self Prefab",
    order = 0)]
public class SpawnSelfPrefabEffect : EffectStrategy
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float destroyDelay = -1;

    public override void StartEffect(AbilityData data, Action finished)
    {
        data.StartCoroutine(Effect(data, finished));
    }

    private IEnumerator Effect(AbilityData data, Action finished)
    {
        GameObject instance = Instantiate(prefabToSpawn);
        instance.transform.position = data.GetUser().transform.position;
        instance.transform.forward = data.GetUser().transform.forward;
        if (destroyDelay > 0)
        {
            yield return new WaitForSeconds(destroyDelay);
            Destroy(instance);
        }

        finished();
    }
}