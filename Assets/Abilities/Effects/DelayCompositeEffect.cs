using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Delay Composite Effect", menuName = "Abilities/Effects/Delay Composite", order = 0)]
public class DelayCompositeEffect : EffectStrategy
{
    [SerializeField] float delay = 0;
    [SerializeField] EffectStrategy[] delayedEffects;
    [SerializeField] bool abortIfCancelled = false;
    [SerializeField] AudioClip audioClip;

    public override void StartEffect(AbilityData data, Action finished)
    {
        data.StartCoroutine(DelayedEffect(data, finished));
    }

    private IEnumerator DelayedEffect(AbilityData data, Action finished)
    {
        yield return new WaitForSeconds(delay);
        if (audioClip != null)
        {
            AudioSource audioSource = data.GetUser().GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioClip);
        }
        if (abortIfCancelled && data.IsCancelled()) yield break;
        foreach (var effect in delayedEffects)
        {
            effect.StartEffect(data, finished);
        }
    }
}
