using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/ Make New Weapon")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] AnimatorOverrideController animatorOverride = null;
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] float weaponDamage = 5f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] bool isRightHanded = true;
    [SerializeField] Projectile projectile = null;

    const string weaponName = "Weapon";

    public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
    {
        DestroyOldWeapon(rightHand, leftHand);

        Weapon weapon = null;

        if (equippedPrefab != null)
        {

            Transform handTransform = GetTransform(rightHand, leftHand);
            weapon = Instantiate(equippedPrefab, handTransform);
            weapon.gameObject.name = weaponName;
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride;
        }

        else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        return weapon;
    }

    private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon = rightHand.Find(weaponName);
        if (oldWeapon == null)
            oldWeapon = leftHand.Find(weaponName);
        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    public Transform GetTransform(Transform rightHand, Transform leftHand)
    {
        Transform handTransform;
        if (isRightHanded) handTransform = rightHand;
        else handTransform = leftHand;
        return handTransform;
    }

    public bool HasProjectile()
    {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage, Transform transform)
    {
        Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, transform.rotation);
        //projectileInstance.SetTarget(target, instigator, calculatedDamage);
    }

    public void LaunchArrow(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage, Transform transform)
    {
        Projectile projectileInstance = Instantiate(projectile, rightHand.position, transform.rotation);
    }

    public float GetDamage()
    {
        return weaponDamage;
    }

    public float GetRange()
    {
        return weaponRange;
    }
}
