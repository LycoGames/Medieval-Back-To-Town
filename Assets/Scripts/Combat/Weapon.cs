using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(PhotonView))] 
public class Weapon : MonoBehaviour
{
    [SerializeField] UnityEvent onHit;

    public void OnHit()
    {
        onHit.Invoke();
    }
}
