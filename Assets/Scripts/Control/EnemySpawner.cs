using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    GameObject[] enemies;
    Vector3[] enemyStartPosition;
    float delayUpdateForOptimization = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(Updater());
        enemies = new GameObject[transform.childCount];
        enemyStartPosition = new Vector3[transform.childCount];
        int columns = transform.childCount;
        print("" + columns);
        for (int i = 0; i < columns; i++)
        {
            enemies[i] = transform.GetChild(i).gameObject;
            enemyStartPosition[i] = transform.GetChild(i).gameObject.transform.position;
            print("pos: " + enemyStartPosition[i]);
            //  print("child: " + enemies[i]);
        }
        //  StartCoroutine(Updater());
    }
    /*
        private IEnumerator Updater()
        {
            while (true)
            {
                yield return new WaitForSeconds(delayUpdateForOptimization);
                Spawner();
            }
        }*/
    public void SpawnEnemy(GameObject enemy)
    {
        for (int i = 0; i < 23; i++)
        {
            print("calıstım");
            if (enemy.GetInstanceID() == enemies[i].GetInstanceID())
            {
                print("im equal");
                Instantiate(enemyPrefab, enemyStartPosition[i], Quaternion.identity, transform);
            }
        }

        print("spawned: " + enemyStartPosition);
    }
    /*
        private void Spawner()
        {
            for (int i = 0; i < 23; i++)
            {
                if (enemies[i].GetComponent<Health>().IsDead())
                {
                    SpawnEnemy(i);
                    print("spawner2");
                }
            }
        }*/
    // Update is called once per frame
    void Update()
    {

    }
}



