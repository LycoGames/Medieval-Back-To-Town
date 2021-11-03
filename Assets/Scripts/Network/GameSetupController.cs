using Photon.Pun;
using UnityEngine;
using System.IO;
using System;

public class GameSetupController : MonoBehaviour
{

    public Transform spawnPoint;

    private static int playerCount = 0;
    [SerializeField] EnemyAIController enemyAi;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {

        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), spawnPoint.position, Quaternion.identity);
        Debug.Log("Player Created:" + player);
        GameObject spider = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Spider Variant"), spawnPoint.position + new Vector3(2, 0, 2), Quaternion.identity);
        Debug.Log("Spider Created:" + spider);
        Debug.Log(PhotonNetwork.CountOfPlayers);
    }

}
