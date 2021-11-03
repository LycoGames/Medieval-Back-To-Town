using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int enemiesAlive;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject enemyPrefab;

    private void Start() {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawners");
    }

    private void Update() {
        if(enemiesAlive == 0){
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for (var x = 0; x < 3; x++)
        {
            GameObject spawnPoint = spawnPoints[x];
            GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            enemySpawned.GetComponent<EnemyAIController>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }
}
