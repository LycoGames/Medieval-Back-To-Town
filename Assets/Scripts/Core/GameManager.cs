using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] int enemiesAlive;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject enemyPrefab;

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        if (Keyboard.current.tKey.isPressed)
        {
            GameObject enemySpawned = Instantiate(enemyPrefab,
                new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)), Quaternion.identity) as GameObject;
            enemySpawned.GetComponent<EnemyAIController>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }

    private void SpawnEnemies()
    {
        for (var x = 0; x < spawnPoints.Length; x++)
        {
            GameObject spawnPoint = spawnPoints[x];
            GameObject enemySpawned =
                Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;

            if (Random.Range(0, 100) > 50)
            {
                enemySpawned.AddComponent<RandomDropper>();
            }
            enemySpawned.GetComponent<EnemyAIController>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }
}