using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    private ConversationManager cm;
    [SerializeField] private float rotateSpeed = 1f;
    private Coroutine activeCoroutine;

    void Start()
    {
        cm = GetComponentInParent<ConversationManager>();
    }

    public void RotateToTalker(GameObject activeTalker)
    {
        activeCoroutine = StartCoroutine(RotateToTalkerCoroutine(activeTalker));
    }

    public void StartRandomTalk()
    {
        cm.StartRandomTalk();
    }

    private IEnumerator RotateToTalkerCoroutine(GameObject activeTalker)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);
        var newDirection = Vector3.zero;
        var targetDirection = activeTalker.transform.position - transform.position;

        do
        {
            // Determine which direction to rotate towards

            // The step size is equal to speed times frame time.
            float singleStep = rotateSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        } while (Quaternion.Angle(Quaternion.Euler(transform.forward), Quaternion.Euler(targetDirection)) > 1f);
    }
}