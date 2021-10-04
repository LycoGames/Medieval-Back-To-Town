using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        Debug.Log("We are connected to the "+PhotonNetwork.CloudRegion + " server!");
    }
    
}
