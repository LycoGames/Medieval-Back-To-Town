using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

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

    }

    private void SpawnEnemies()
    {
        for (var x = 0; x < spawnPoints.Length; x++)
        {
            GameObject spawnPoint = spawnPoints[x];
            GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
            enemySpawned.GetComponent<EnemyAIController>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }
}
