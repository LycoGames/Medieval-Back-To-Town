using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Archer : MonoBehaviour
{
    [SerializeField] private float maxShootPower;
    [SerializeField] private float shootPowerSpeed;
    [SerializeField] Transform spawnPoint;
    private float shootPower;
    private bool shoot;
    private float firePower = 50f;

    // Start is called before the first frame update
void Start()
{

    if (Input.GetMouseButtonDown(0))
    {
        shoot = true;
    }

    if (shoot && shootPower < maxShootPower)
    {
        shootPower += Time.deltaTime * shootPowerSpeed;
    }

    if (shoot && Input.GetMouseButtonUp(0))
    {
        WeaponConfig.Fire(shootPower);
        shootPower = 0;
        shoot = false;
    }

}


public Vector3 getForce()
{
    var force = spawnPoint.TransformDirection(Vector3.forward * firePower);
   // currentArrow.Fly(force);
    return force;
}


// Update is called once per frame
void Update()
{
   // print("the spawn point is:"+spawnPoint.position);
  //  print("on archer the vector is:"+getForce());
}
}
*/