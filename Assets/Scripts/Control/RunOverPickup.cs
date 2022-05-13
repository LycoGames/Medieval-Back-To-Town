using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class RunOverPickup : MonoBehaviour
{
    ItemPickupAlert ItemPickupAlert;
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    void Start()
    {
        ItemPickupAlert = FindObjectOfType<ItemPickupAlert>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (other.gameObject == player)
        {
            GetComponent<Pickup>().PickupItem();
            ItemPickupAlert.SetText(GetComponent<Pickup>().GetItem().name);
            audioSource = player.GetComponent<AudioSource>();
            //audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioClip);
        }
    }
}
