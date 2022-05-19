using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class Worker : MonoBehaviour
{
    [SerializeField] public Animation[] workAnimations;
    [SerializeField] public float minWorkingTime = 2f;
    [SerializeField] public float maxWorkingTime = 4f;
}