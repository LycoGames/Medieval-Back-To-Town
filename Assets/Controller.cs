using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    ParticleSystem trail;
    // Start is called before the first frame update
    void Start()
    {
        trail = transform.Find("Trail").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
    }

    public void PlayTrail()
    {
        trail.Play();
        Debug.Log("is have trail" + trail.name);
    }

}
