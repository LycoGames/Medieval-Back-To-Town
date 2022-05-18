using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingWorker : Worker
{
    // Start is called before the first frame update
    void Start()
    {
        int randAnimIndex = UnityEngine.Random.Range(0, workAnimations.Length);
        workAnimations[randAnimIndex].Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
}