using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker_Manager : MonoBehaviour
{
    public Transform markersParent;
    /*public string targetTag = "Enemy";*/

    public int damage = 50;

    public GameObject blood;
    private Transform parent;

    bool hitFlesh;

    Marker[] markers;

    List<Transform> targetsRawHit = new List<Transform>(); //Targets initialy hit by the blade (pre-check
    List<Transform> usedTargets = new List<Transform>(); //Targets which were excluded from being hit or were already hit in that frame

    List<Vector3> bladeDirection = new List<Vector3>();
    List<Vector3> bladeStartpoint = new List<Vector3>();

    bool markersAreEnabled;
    GameObject missSparks;

    Health rawTargetInstance;

    public bool startActivated = true; //Should the Markers be active upon the Start of this weapon?

    public AudioSource soundSource;

    [Range(0, 5)]
    public int numberOfTargetHitSounds;
    public AudioClip targetHitSound1;
    public AudioClip targetHitSound2;
    public AudioClip targetHitSound3;
    public AudioClip targetHitSound4;
    public AudioClip targetHitSound5;

    public bool DisableMarkersOnObjectDisable = true;

    private int sRoll;

    private void Start()
    {
        parent = transform.root.GetChild(0);
        if (markersParent == null)
        {
            markersParent = transform;
        }
        markers = new Marker[markersParent.childCount];

        for (int i = 0; i < markers.Length; i++)
        {
            markers[i] = markersParent.GetChild(i).gameObject.GetComponent<Marker>();
        }

        if (startActivated)
        {
            EnableMarkers();
        }
    }

    public void EnableMarkers()
    {
        markersAreEnabled = true;
        ClearLists();
        for (int i2 = 0; i2 < markers.Length; i2++)
        {
            markers[i2].tempPos = markers[i2].transform.position;
            if (i2 > markers.Length)
            {
                i2 = 0;
            }
        }
    }

    public void DisableMarkers()
    {
        markersAreEnabled = false;
        for (int i2 = 0; i2 < markers.Length; i2++)
        {
            markers[i2].tempPos = markers[i2].transform.position;
            if (i2 > markers.Length)
            {
                i2 = 0;
            }
        }
        ClearLists();
        for (int i2 = 0; i2 < markers.Length; i2++)
        {
            markers[i2].tempPos = markers[i2].transform.position;
            if (i2 > markers.Length)
            {
                i2 = 0;
            }
        }
    }

    private void ClearLists()
    {
        targetsRawHit.Clear();
        bladeStartpoint.Clear();
        bladeDirection.Clear();
        usedTargets.Clear();
    }

    private void OnDisable()
    {
        if (DisableMarkersOnObjectDisable)
        {
            DisableMarkers();
        }
    }

    public void ClearTargets()
    {
        targetsRawHit.Clear();
        bladeStartpoint.Clear();
        bladeDirection.Clear();
        usedTargets.Clear();
        for (int i2 = 0; i2 < markers.Length; i2++)
        {
            markers[i2].tempPos = markers[i2].transform.position;
            if (i2 > markers.Length)
            {
                i2 = 0;
            }
        }
    }

    private void Update()
    {
        if (markersAreEnabled)
        {
            int i;
            for (i = 0; i < markers.Length; i++)
            {
                if (markers[i].HitCheck() != null)
                {
                    if (/*markers[i].target.tag == targetTag && */ markers[i].target != parent && targetsRawHit.Contains(markers[i].target) == false && usedTargets.Contains(markers[i].target) == false)
                    {
                        bladeDirection.Add(markers[i].tempPos);
                        bladeStartpoint.Add(markers[i].hit.point);
                        hitFlesh = true;
                        if (markers[i].target.GetComponent<Health>() != null)
                        {
                            rawTargetInstance = markers[i].target.GetComponent<Health>();
                        }
                        if (markers[i].target.GetComponent<Limb_Hitbox>() != null)
                        {
                            rawTargetInstance = markers[i].target.GetComponent<Limb_Hitbox>().health;
                            usedTargets.Add(markers[i].target.transform);
                        }
                        if (rawTargetInstance != null)
                        {
                            targetsRawHit.Add(rawTargetInstance.transform);
                        }
                    }
                }
            }
            if (i > markers.Length)
            {
                i = 0;
            }

        }

        // Dealing Damage

        //Shield Part

        if (hitFlesh)
        {
            for (int i2 = 0; i2 < targetsRawHit.Count; i2++)
            {
                if (targetsRawHit[i2] != null && targetsRawHit[i2].GetComponent<Health>() != null && usedTargets.Contains(targetsRawHit[i2]) == false)
                {
                    targetsRawHit[i2].GetComponent<Health>().Bloodflood(bladeDirection[i2], bladeStartpoint[i2]);

                    PlayTargetHitSound();

                    if (targetsRawHit[i2].GetComponent<Health>() != null)
                    {
                        targetsRawHit[i2].GetComponent<Health>().ApplyDamage(damage);
                    }

                    if (blood != null)
                    {
                        GameObject b = Instantiate(blood, bladeStartpoint[i2], Quaternion.identity) as GameObject;
                        b.transform.LookAt(markersParent);
                    }
                    usedTargets.Add(targetsRawHit[i2]);
                }
                if (targetsRawHit[i2] != null && targetsRawHit[i2].GetComponent<Limb_Hitbox>() != null && usedTargets.Contains(targetsRawHit[i2]) == false)
                {
                    targetsRawHit[i2].GetComponent<Limb_Hitbox>().health.Bloodflood(bladeDirection[i2], bladeStartpoint[i2]);

                    PlayTargetHitSound();

                    targetsRawHit[i2].GetComponent<Limb_Hitbox>().health.ApplyDamage(damage);

                    if (blood != null)
                    {
                        GameObject b = Instantiate(blood, bladeStartpoint[i2], Quaternion.identity) as GameObject;
                        b.transform.LookAt(markersParent);
                    }
                    usedTargets.Add(targetsRawHit[i2].GetComponent<Limb_Hitbox>().health.transform);
                }
            }
        }
    }

    void PlayTargetHitSound()
    {
        if (soundSource != null)
        {
            if (numberOfTargetHitSounds > 0)
            {
                sRoll = UnityEngine.Random.Range(1, numberOfTargetHitSounds + 1);

                if (sRoll == 1)
                {
                    soundSource.PlayOneShot(targetHitSound1);

                }
                if (sRoll == 2)
                {
                    soundSource.PlayOneShot(targetHitSound2);

                }
                if (sRoll == 3)
                {
                    soundSource.PlayOneShot(targetHitSound3);

                }
                if (sRoll == 4)
                {
                    soundSource.PlayOneShot(targetHitSound4);

                }
                if (sRoll == 5)
                {
                    soundSource.PlayOneShot(targetHitSound5);

                }

            }
        }
    }

    /* SHIELD HIT SOUND  */

}
