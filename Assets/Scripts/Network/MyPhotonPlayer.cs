using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MyPhotonPlayer : MonoBehaviour
{
    PhotonView myPV;
    GameObject myPlayer;

    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
        {
            myPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerPrefab"), new Vector3(-5, 0, -5), Quaternion.identity);
        }
    }
}
