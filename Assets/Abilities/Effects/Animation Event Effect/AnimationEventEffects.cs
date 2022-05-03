using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AnimationEventEffects : MonoBehaviour
{
    //public GameObject EffectPrefab;
    //public Transform EffectStartPosition;
    //public float DestroyAfter = 10;
    //[Space]
    //public GameObject EffectPrefabWorldSpace;
    //public Transform EffectStartPositionWorld;
    //public float DestroyAfterWorld = 10;


    public AbilityInfo[] Abilities;
    public float tpDistance = 10;

    private const int ATTEMPTS = 50;


    [System.Serializable]
    public class AbilityInfo
    {
        public EffectInfo[] AbilityEffects;
    }

    [System.Serializable]
    public class EffectInfo
    {
        public GameObject Effect;
        public Transform StartPositionRotation;
        public float DestroyAfter = 10;
        public bool UseLocalPosition = true;
        public float damage;
    }

    //   // Update is called once per frame
    //   void CreateEffect () {
    //       var effectOBJ = Instantiate(EffectPrefab, EffectStartPosition);
    //       effectOBJ.transform.localPosition = Vector3.zero;
    //       Destroy(effectOBJ, DestroyAfter);        		
    //}

    //   void CreateEffectWorldSpace()
    //   {
    //       var effectOBJ = Instantiate(EffectPrefabWorldSpace, EffectStartPositionWorld.transform.position, EffectStartPositionWorld.transform.rotation);

    //       Destroy(effectOBJ, DestroyAfterWorld);
    //   }

    void InstantiateEffect(string data)
    {
        int abilityNumber = Convert.ToInt32(data.Substring(0, 1));
        int EffectNumber = Convert.ToInt32(data.Substring(1, 1));
        EffectInfo[] effects = Abilities[abilityNumber].AbilityEffects;
        if (effects == null || effects.Length <= EffectNumber)
        {
            Debug.LogError("Incorrect effect number or effect is null");
        }

        if (effects != null)
        {
            var instance = Instantiate(effects[EffectNumber].Effect,
                effects[EffectNumber].StartPositionRotation.position,
                effects[EffectNumber].StartPositionRotation.rotation);

            if (effects[EffectNumber].UseLocalPosition)
            {
                instance.transform.parent = effects[EffectNumber].StartPositionRotation.transform;
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = new Quaternion();
            }

            EffectDamage effectDamage = instance.GetComponentInChildren<EffectDamage>();
            if (effectDamage != null)
                effectDamage.SetDamage(effects[EffectNumber].damage);

            Destroy(instance, effects[EffectNumber].DestroyAfter);
        }
    }

    void CustomTeleport()
    {
        Vector3 tpPoint = transform.position + transform.forward * tpDistance;
        NavMeshHit hit;
        for (int i = 0; i < ATTEMPTS; i++)
        {
            if (NavMesh.SamplePosition(tpPoint, out hit, 10f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                break;
            }
        }
    }
}