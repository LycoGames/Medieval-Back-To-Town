using System;
using Photon.Pun;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public static Fighter localPlayer;
    
    //Network
    PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
            localPlayer = this;


        if (!myPV.IsMine)
            return;

    }

    // Update is called once per frame
    void Update()
    {
        if (!myPV.IsMine) { return; }
    }


}
