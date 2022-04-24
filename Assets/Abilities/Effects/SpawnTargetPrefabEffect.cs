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
    [SerializeField] bool shouldSpawnOnHand;
    [SerializeField] bool staticSpawnPoint;
    GameObject player;

    Vector3 posX;


    public override void StartEffect(AbilityData data, Action finished)
    {
        data.StartCoroutine(Effect(data, finished));
    }

    private IEnumerator UpdateX()
    {
        while (true)
        {
            Debug.Log("konum " + posX);
            yield return null;
        }
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
            if (staticSpawnPoint)
            {
                Debug.Log("asdffsda");
                instance = Instantiate(prefabToSpawn);
                instance.position = data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform().position;
            }
            else { instance = Instantiate(prefabToSpawn, data.GetUser().GetComponent<StateMachine>().GetLeftHandTransform()); Debug.Log("aaaaa"); }

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