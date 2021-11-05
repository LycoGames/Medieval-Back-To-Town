using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float regenerationPercentage = 50;
    [SerializeField] UnityEvent onDie;
    [SerializeField] UnityEvent takeDamage;

    LazyValue<float> healthPoints;

    public Animator hurtAnimator;
    public Animation hurtLegacyAnimation;
    [Range(0, 5)]
    public int numberOfHurtAnimations;
    public GameObject blood;

    public AudioSource soundSource;

    public int numberOfHurtSounds;
    public AudioClip hurtSound1;
    public AudioClip hurtSound2;
    public AudioClip hurtSound3;
    public AudioClip hurtSound4;
    public AudioClip hurtSound5;

    [Range(0, 5)]
    public int numberOfDeathSounds;
    public AudioClip deathSound1;
    public AudioClip deathSound2;
    public AudioClip deathSound3;
    public AudioClip deathSound4;
    public AudioClip deathSound5;


    public int numberOfDeathAnimations;
    public Animator deathAnimator;
    public Animation deathLegacyAnimation;


    public bool destroyOnDeath = false;
    public float destroyDelay;

    //bool isDead = false;

    GameObject bloodInstance;
    int hurtAnimRandomisation;
    int deathAnimRandomisation;
    int hurtSoundRandomisation;

    private bool isDead = false;

    private void Awake()
    {
        healthPoints = new LazyValue<float>(GetInitialHealth);
    }

    public float GetInitialHealth()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    private void Start()
    {
        healthPoints.ForceInit();
    }

    private void OnEnable()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
    }
    private void OnDisable()
    {
        GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
    }

    private void RegenerateHealth()
    {
        float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
        healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
    }

    public float GetHealthPoints()
    {
        return healthPoints.value;
    }

    public void Bloodflood(Vector3 prevMarkerPos, Vector3 hitPos)   //Instantiate blood in the direction of the marker which hit this object.
    {
        if (blood != null && healthPoints.value > 0)
        {
            bloodInstance = Instantiate(blood, hitPos, transform.rotation) as GameObject;
            bloodInstance.transform.LookAt(2 * bloodInstance.transform.position - prevMarkerPos);

        }

    }

    public void ApplyDamage(float dmg)   //Let's apply some damage on hit, shall we?
    {
        healthPoints.value = Mathf.Max(healthPoints.value - dmg, 0);
        Debug.Log(healthPoints.value);

        if (healthPoints.value == 0)
        {
            Die();
        }

        else
        {
            takeDamage.Invoke(); //event
            Debug.Log("hasar verildi.");
        }





        if (healthPoints.value > 0)
        {
            if (numberOfHurtAnimations != 0)
            {
                hurtAnimRandomisation = Random.Range(1, numberOfHurtAnimations + 1);   //Hurt animations
            }
            if (numberOfHurtAnimations == 0)
            {
                hurtAnimRandomisation = 0;
            }

            if (hurtAnimator != null)
            {
                if (hurtAnimRandomisation == 1)
                {
                    hurtAnimator.Play("Hurt1");
                }
                if (hurtAnimRandomisation == 2)
                {
                    hurtAnimator.Play("Hurt2");
                }
                if (hurtAnimRandomisation == 3)
                {
                    hurtAnimator.Play("Hurt3");
                }
                if (hurtAnimRandomisation == 4)
                {
                    hurtAnimator.Play("Hurt4");
                }
                if (hurtAnimRandomisation == 5)
                {
                    hurtAnimator.Play("Hurt5");
                }
            }
            if (hurtLegacyAnimation != null)
            {
                if (hurtAnimRandomisation == 1)
                {
                    hurtLegacyAnimation.Play("Hurt1");
                }
                if (hurtAnimRandomisation == 2)
                {
                    hurtLegacyAnimation.Play("Hurt2");
                }
                if (hurtAnimRandomisation == 3)
                {
                    hurtLegacyAnimation.Play("Hurt3");
                }
                if (hurtAnimRandomisation == 4)
                {
                    hurtLegacyAnimation.Play("Hurt4");
                }
                if (hurtAnimRandomisation == 5)
                {
                    hurtLegacyAnimation.Play("Hurt5");
                }
            }

            if (soundSource != null)
            {
                if (numberOfHurtSounds > 0)
                {
                    hurtSoundRandomisation = Random.Range(1, numberOfHurtSounds + 1);

                    if (hurtSoundRandomisation == 1)
                    {
                        soundSource.clip = hurtSound1;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 2)
                    {
                        soundSource.clip = hurtSound2;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 3)
                    {
                        soundSource.clip = hurtSound3;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 4)
                    {
                        soundSource.clip = hurtSound4;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 5)
                    {
                        soundSource.clip = hurtSound5;
                        soundSource.Play();
                    }

                }
            }

        }

        if (healthPoints.value <= 0)//Death,
        {
            destroyOnDeath = true;
            //Spawn Of Death

            //components

            //Objects

            //This one is about sending a message...

            if (numberOfDeathAnimations != 0)
            {
                deathAnimRandomisation = Random.Range(1, numberOfDeathAnimations + 1);   //Hurt animations
            }
            if (numberOfDeathAnimations == 0)
            {
                deathAnimRandomisation = 0;
            }

            if (deathAnimator != null)
            {
                if (deathAnimRandomisation == 1)
                {
                    deathAnimator.Play("Death1");
                }
                if (deathAnimRandomisation == 2)
                {
                    deathAnimator.Play("Death2");
                }
                if (deathAnimRandomisation == 3)
                {
                    deathAnimator.Play("Death3");
                }
                if (deathAnimRandomisation == 4)
                {
                    deathAnimator.Play("Death4");
                }
                if (deathAnimRandomisation == 5)
                {
                    deathAnimator.Play("Death5");
                }
            }



            if (deathLegacyAnimation != null)
            {
                if (deathAnimRandomisation == 1)
                {
                    deathLegacyAnimation.Play("Death1");
                }
                if (deathAnimRandomisation == 2)
                {
                    deathLegacyAnimation.Play("Death2");
                }
                if (deathAnimRandomisation == 3)
                {
                    deathLegacyAnimation.Play("Death3");
                }
                if (deathAnimRandomisation == 4)
                {
                    deathLegacyAnimation.Play("Death4");
                }
                if (deathAnimRandomisation == 5)
                {
                    deathLegacyAnimation.Play("Death5");
                }
            }

            if (soundSource != null)
            {
                if (numberOfDeathSounds > 0)
                {
                    hurtSoundRandomisation = Random.Range(1, numberOfDeathSounds + 1);

                    if (hurtSoundRandomisation == 1)
                    {
                        soundSource.clip = deathSound1;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 2)
                    {
                        soundSource.clip = deathSound2;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 3)
                    {
                        soundSource.clip = deathSound3;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 4)
                    {
                        soundSource.clip = deathSound4;
                        soundSource.Play();
                    }
                    if (hurtSoundRandomisation == 5)
                    {
                        soundSource.clip = deathSound5;
                        soundSource.Play();
                    }

                }
            }

            if (destroyOnDeath)
            {
                Destroy(gameObject.GetComponent<Fighter>().GetCurrentWeapon(), destroyDelay);
                Destroy(gameObject, destroyDelay);
            }
        }

    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
