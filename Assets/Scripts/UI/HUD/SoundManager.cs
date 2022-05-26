using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource[] sources;
    public AudioClip[] audioClips;
    StateMachine stateMachine;
    WStateMachine wStateMachine;
    BaseState state;

    WBaseState wstate;

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        stateMachine = GetComponent<StateMachine>();
        wStateMachine = GetComponent<WStateMachine>();
    }

    public void Step()
    {
        if (stateMachine != null)
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
        else
        {
            wstate = wStateMachine.CurrentState?.currentSubState?.currentSubState?.currentSubState;
            if (wstate is WWalkState || wstate is WRunState)
            {
                AudioClip clip = GetRandomClip();
                sources[1].clip = clip;
                sources[1].volume = UnityEngine.Random.Range(0.5f, 0.8f);
                sources[1].pitch = UnityEngine.Random.Range(0.8f, 1.1f);
                sources[1].PlayOneShot(clip);
            }
        }
    }

    private AudioClip GetRandomClip()
    {
        return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    }
}