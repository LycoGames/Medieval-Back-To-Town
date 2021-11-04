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

    /*Network Variables*/
    public PhotonView photonView;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawners");
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            if (enemiesAlive == 0)
            {
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()
    {
        for (var x = 0; x < 3; x++)
        {
            GameObject spawnPoint = spawnPoints[x];
            GameObject enemySpawned;

            enemySpawned = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spider Variant"), spawnPoint.transform.position, Quaternion.identity);
            Debug.Log(enemySpawned);
            enemySpawned.GetComponent<EnemyAIController>().gameManager = GetComponent<GameManager>();
            enemiesAlive++;
        }
    }
}
