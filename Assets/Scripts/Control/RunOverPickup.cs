using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class RunOverPickup : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    private void OnTriggerEnter(Collider other)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (other.gameObject == player)
        {
            GetComponent<Pickup>().PickupItem();
            audioSource = player.GetComponent<AudioSource>();
            //audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioClip);
            //TODO playoneshot audio
        }
    }
}
