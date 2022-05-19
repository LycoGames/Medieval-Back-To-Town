using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private Animator[] talkers;
    [SerializeField] private float rotateSpeed = 1f;
    private int activeTalkerIndex = -1;

    private void Start()
    {
        StartRandomTalk();
    }

    public void StartRandomTalk()
    {
        activeTalkerIndex = UnityEngine.Random.Range(0, talkers.Length);
        var randomAnimNumber = UnityEngine.Random.Range(1, 4);
        switch (randomAnimNumber)
        {
            case 1:
                talkers[activeTalkerIndex].SetTrigger("Talk1");
                break;
            case 2:
                talkers[activeTalkerIndex].SetTrigger("Talk2");
                break;
            case 3:
                talkers[activeTalkerIndex].SetTrigger("Talk3");
                break;
        }

        if (talkers.Length > 1)
            MakeListenersLookTalker();
    }

    private void MakeListenersLookTalker()
    {
        for (int i = 0; i < talkers.Length; i++)
        {
            if (i == activeTalkerIndex)
                continue;
            talkers[i].GetComponent<Talker>().RotateToTalker(talkers[activeTalkerIndex].gameObject);
        }
    }
}