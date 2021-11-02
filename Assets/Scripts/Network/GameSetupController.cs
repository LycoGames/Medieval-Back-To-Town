using Photon.Pun;
using UnityEngine;
using System.IO;
using System;

public class GameSetupController : MonoBehaviour
{

    public Transform spawnPoint;
    [SerializeField] EnemyAIController enemyAi;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), spawnPoint.position, Quaternion.identity);
    }

}
