using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Animation_Events : MonoBehaviour
{
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    public Marker_Manager MarkerManager;

    const string weaponName = "Weapon";

    void Start()
    {
        Transform weapon = rightHand.Find(weaponName);
        if (weapon == null)
            weapon = leftHand.Find(weaponName);
        if (weapon == null) return;

        MarkerManager = weapon.GetChild(weapon.childCount-1).GetComponent<Marker_Manager>();
    }


    public void ClearTargets()
    {
        MarkerManager.ClearTargets();
    }

    public void DisableMarkers()
    {
        MarkerManager.DisableMarkers();
    }
    public void EnableMarkers()
    {
        MarkerManager.EnableMarkers();
    }
}
