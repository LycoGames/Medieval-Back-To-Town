using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Potion"))]
public class PotionItem : ActionItem
{
    [SerializeField] private float cooldownTime = 1f;

    [Header("For One Time Effect Potion Attirbutes")] [SerializeField]
    private bool increaseHealthPoints;

    [SerializeField] private bool increaseManaPoints;
    [SerializeField] private int value;

    [Header("For Long Term Effect Potion Attirbutes")]
    [Tooltip("The value that directly affects the status.")]
    [SerializeField]
    private Modifier[] additiveModifiers;

    [Tooltip("The value that affects the status as a percentage.")] [SerializeField]
    private Modifier[] percentageModifiers;

    [Tooltip("Unit is Second, The effect takes effect as long as the entered time.")] [SerializeField]
    private float impactTime;

    [Tooltip(
        "VFX that will be active for the duration of the pot's duration. Vfx will work both one time and long term effect potions")]
    [SerializeField]
    private GameObject vfxPrefab;

    [SerializeField] private AudioClip useSfx;

    [System.Serializable]
    public struct Modifier
    {
        public Stat stat;
        public float value;
    }

    public override bool Use(GameObject user)
    {
        base.Use(user);

        CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
        if (cooldownStore.GetTimeRemaining(this) > 0)
        {
            return false;
        }
        
        if (increaseHealthPoints || increaseManaPoints)
        {
            if (increaseHealthPoints)
                user.GetComponent<Health>().Heal(value);
            else
                user.GetComponent<Mana>().AddMana(value);
        }

        if (useSfx != null)
        {
            var sources = user.GetComponents<AudioSource>();
            sources[1].PlayOneShot(useSfx);
        }

        var potionEffect = user.GetComponent<PotionEffect>();
        potionEffect.Setup(impactTime, additiveModifiers, percentageModifiers);
        potionEffect.StartCoroutine(StartEffect(user));
        cooldownStore.StartCooldown(this, cooldownTime);
        return true;
    }

    private IEnumerator StartEffect(GameObject user)
    {
        GameObject vfx = null;
        if (vfxPrefab != null)
        {
            vfx = Instantiate(vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
        }

        yield return new WaitForSeconds(impactTime);
        if (vfx != null)
        {
            Destroy(vfx);
        }
    }
}