using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Potion"))]
public class PotionItem : ActionItem
{
    [Tooltip("The value that directly affects the status.")] [SerializeField]
    private Modifier[] additiveModifiers;

    [Tooltip("The value that affects the status as a percentage.")] [SerializeField]
    private Modifier[] percentageModifiers;

    [Tooltip("Unit is Second, The effect takes effect as long as the entered time.")] [SerializeField]
    private float impactTime;

    [Tooltip("VFX that will be active for the duration of the pot's duration")] [SerializeField]
    private GameObject vfxPrefab;

    [System.Serializable]
    public struct Modifier
    {
        public Stat stat;
        public float value;
    }

    public override void Use(GameObject user)
    {
        base.Use(user);
        var potionEffect = user.GetComponent<PotionEffect>();
        potionEffect.Setup(impactTime, additiveModifiers, percentageModifiers);
        potionEffect.StartCoroutine(StartEffect(user));
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