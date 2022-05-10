using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    AudioSource[] sources;
    public AudioClip[] audioClips;
    StateMachine stateMachine;
    BaseState state;
    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        stateMachine = GetComponent<StateMachine>();
    }

    public void Step()
    {
        state = stateMachine.CurrentState.currentSubState.currentSubState.currentSubState;
        if (state is WalkState || state is RunState)
        {
            AudioClip clip = GetRandomClip();
            sources[1].clip = clip;
            sources[1].volume = UnityEngine.Random.Range(0.5f, 0.8f);
            sources[1].pitch = UnityEngine.Random.Range(0.8f, 1.1f);
            sources[1].PlayOneShot(clip);
        }

    }

    private AudioClip GetRandomClip()
    {
        return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    }
}
