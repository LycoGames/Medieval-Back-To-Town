using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    public string[] enemies;
    int numberOfEnemies;
    Vector3[] enemyStartPosition;
    public int enemyRespawnCooldown = 10;

    // Start is called before the first frame update
    void Start()
    {
        numberOfEnemies = transform.childCount;
        enemies = new string[transform.childCount];
        enemyStartPosition = new Vector3[transform.childCount];
        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemies[i] = transform.GetChild(i).name;
            enemyStartPosition[i] = transform.GetChild(i).gameObject.transform.position;
        }
    }

    public void SpawnEnemy(GameObject enemy)
    {
        StartCoroutine(SpawnCoroutine(enemy));
    }

    IEnumerator SpawnCoroutine(GameObject enemy)
    {
        int indexOfEnemy = 0;
        string enemyName = enemy.name;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (enemyName == enemies[i])
            {
                indexOfEnemy = i;
            }
        }
        yield return new WaitForSeconds(enemyRespawnCooldown);
        GameObject newEnemy = Instantiate(enemyPrefab, enemyStartPosition[indexOfEnemy], Quaternion.identity, transform);
        newEnemy.name = enemies[indexOfEnemy]; // set died enemy's name original name
    }
}



