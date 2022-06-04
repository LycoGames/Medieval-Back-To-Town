using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelfRespawner : MonoBehaviour
{
    public static SelfRespawner sharedInstance;

    public float respawnCooldown = 1f;

    WaitForSeconds respawnCooldownWaitForSecons;

    public GameObject centaur;
    public GameObject hpGolem;
    public GameObject pbrGolem;
    public GameObject iceGolem;
    public GameObject fireGolem;
    public GameObject skeleton;
    public GameObject spider;
    public GameObject troll;
    public GameObject bigSpider;

    void Awake()
    {
        sharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        respawnCooldownWaitForSecons = new WaitForSeconds(respawnCooldown);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnSelf(Vector3 myPosition, Quaternion rotation, Enemies type)
    {
        switch (type)
        {
            case Enemies.centaur:
                StartCoroutine(DelayedRespawn(myPosition, rotation, centaur));
                break;
            case Enemies.fireGolem:
                StartCoroutine(DelayedRespawn(myPosition, rotation, fireGolem));
                break;
            case Enemies.hpGolem:
                StartCoroutine(DelayedRespawn(myPosition, rotation, hpGolem));
                break;
            case Enemies.iceGolem:
                StartCoroutine(DelayedRespawn(myPosition, rotation, iceGolem));
                break;
            case Enemies.pbrGolem:
                StartCoroutine(DelayedRespawn(myPosition, rotation, pbrGolem));
                break;
            case Enemies.skeleton:
                StartCoroutine(DelayedRespawn(myPosition, rotation, skeleton));
                break;
            case Enemies.spider:
                StartCoroutine(DelayedRespawn(myPosition, rotation, spider));
                break;
            case Enemies.troll:
                StartCoroutine(DelayedRespawn(myPosition, rotation, troll));
                break;
            case Enemies.spiderBig:
                StartCoroutine(DelayedRespawn(myPosition, rotation, bigSpider));
                break;
        }

    }

    IEnumerator DelayedRespawn(Vector3 enemyPosition, Quaternion rotation, GameObject objectToInstantiate)
    {

        yield return respawnCooldownWaitForSecons;
        Instantiate(objectToInstantiate, enemyPosition, rotation,transform);
    }
}
