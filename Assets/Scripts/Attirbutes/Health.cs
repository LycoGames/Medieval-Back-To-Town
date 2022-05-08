using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour
{
    [SerializeField] float regenerationPercentage = 50;
    public UnityEvent onDie;
    public UnityEvent takeDamage;

    public TakeDamageHUDEvent takeDamageHUD;

    [System.Serializable]
    public class TakeDamageHUDEvent : UnityEvent<float>
    {
    }

    LazyValue<float> healthPoints;

    public Animator hurtAnimator;
    public Animation hurtLegacyAnimation;
    [Range(0, 5)] public int numberOfHurtAnimations;
    public GameObject blood;

    public AudioSource soundSource;

    public int numberOfHurtSounds;
    public AudioClip hurtSound1;
    public AudioClip hurtSound2;
    public AudioClip hurtSound3;
    public AudioClip hurtSound4;
    public AudioClip hurtSound5;

    [Range(0, 5)] public int numberOfDeathSounds;
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
    private HealthBar healthBar;

    bool wasDeadLastFrame = false;

    GameObject bloodInstance;
    int hurtAnimRandomisation;
    int deathAnimRandomisation;
    int hurtSoundRandomisation;

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
        healthBar = GetComponent<HealthBar>();
        if (healthBar)
        {
            healthBar.SetMaxHealth(GetInitialHealth());
        }

        onDie.AddListener(UpdateState);
    }

    private void OnEnable()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        GetComponent<BaseStats>().onLevelUp += SetNewMaxHealthOnHUD;
    }

    private void OnDisable()
    {
        GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        GetComponent<BaseStats>().onLevelUp -= SetNewMaxHealthOnHUD;
    }

    private void RegenerateHealth()
    {
        float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
        healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        healthBar.SetHealth(GetHealthPoints());
    }

    public float GetHealthPoints()
    {
        return healthPoints.value;
    }

    public float GetMaxHealthPoints()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    public void SetNewMaxHealthOnHUD()
    {
        if (healthBar)
        {
            healthBar.SetNewMaxHealth(GetMaxHealthPoints());
        }
    }


    public void
        Bloodflood(Vector3 prevMarkerPos,
            Vector3 hitPos) //Instantiate blood in the direction of the marker which hit this object.
    {
        if (blood != null && healthPoints.value > 0)
        {
            bloodInstance = Instantiate(blood, hitPos, transform.rotation) as GameObject;
            bloodInstance.transform.LookAt(2 * bloodInstance.transform.position - prevMarkerPos);
        }
    }

    public void ApplyDamage(GameObject insigator, float dmg) //Let's apply some damage on hit, shall we?
    {
        if (IsDead()) return;
        float defencePoint = GetComponent<BaseStats>().GetStat(Stat.Defence);
        dmg = Mathf.Max(dmg - defencePoint, 0);
        //TODO eğer 0 damage atarsa miss yazsın
        healthPoints.value = Mathf.Max(healthPoints.value - dmg, 0);

        takeDamage.Invoke(); //event
        takeDamageHUD.Invoke(dmg);

        healthBar.SetHealth(GetHealthPoints());

        if (healthPoints.value > 0)
        {
            if (numberOfHurtAnimations != 0)
            {
                hurtAnimRandomisation = Random.Range(1, numberOfHurtAnimations + 1); //Hurt animations
            }

            if (numberOfHurtAnimations == 0)
            {
                hurtAnimRandomisation = 0;
            }

            if (hurtAnimator != null && AnimatorIsPlaying("Locomotion"))
            {
                switch (hurtAnimRandomisation)
                {
                    case 1:
                        hurtAnimator.Play("Hurt1");
                        break;
                    case 2:
                        hurtAnimator.Play("Hurt2");
                        break;
                    case 3:
                        hurtAnimator.Play("Hurt3");
                        break;
                    case 4:
                        hurtAnimator.Play("Hurt4");
                        break;
                    case 5:
                        hurtAnimator.Play("Hurt5");
                        break;
                }
            }

            if (hurtLegacyAnimation != null)
            {
                switch (hurtAnimRandomisation)
                {
                    case 1:
                        hurtLegacyAnimation.Play("Hurt1");
                        break;
                    case 2:
                        hurtLegacyAnimation.Play("Hurt2");
                        break;
                    case 3:
                        hurtLegacyAnimation.Play("Hurt3");
                        break;
                    case 4:
                        hurtLegacyAnimation.Play("Hurt4");
                        break;
                    case 5:
                        hurtLegacyAnimation.Play("Hurt5");
                        break;
                }
            }

            if (soundSource != null)
            {
                if (numberOfHurtSounds > 0)
                {
                    hurtSoundRandomisation = Random.Range(1, numberOfHurtSounds + 1);

                    switch (hurtSoundRandomisation)
                    {
                        case 1:
                            soundSource.clip = hurtSound1;
                            soundSource.Play();
                            break;
                        case 2:
                            soundSource.clip = hurtSound2;
                            soundSource.Play();
                            break;
                        case 3:
                            soundSource.clip = hurtSound3;
                            soundSource.Play();
                            break;
                        case 4:
                            soundSource.clip = hurtSound4;
                            soundSource.Play();
                            break;
                        case 5:
                            soundSource.clip = hurtSound5;
                            soundSource.Play();
                            break;
                    }
                }
            }
        }

        if (IsDead()) //Death,
        {
            onDie.Invoke();
            if (CompareTag("Enemy"))
            {
                insigator.GetComponent<QuestList>()
                    .UpdateKillObjectiveStatus(gameObject, 1);
            }

            AwardExperience(insigator);
            //Spawn Of Death

            //components

            //Objects

            //This one is about sending a message...

            if (numberOfDeathAnimations != 0)
            {
                deathAnimRandomisation = Random.Range(1, numberOfDeathAnimations + 1); //Hurt animations
            }

            if (numberOfDeathAnimations == 0)
            {
                deathAnimRandomisation = 0;
            }

            if (deathAnimator != null)
            {
                deathAnimator.applyRootMotion = true;
                switch (deathAnimRandomisation)
                {
                    case 1:
                        deathAnimator.Play("Death1");
                        break;
                    case 2:
                        deathAnimator.Play("Death2");
                        break;
                    case 3:
                        deathAnimator.Play("Death3");
                        break;
                    case 4:
                        deathAnimator.Play("Death4");
                        break;
                    case 5:
                        deathAnimator.Play("Death5");
                        break;
                }
            }


            if (deathLegacyAnimation != null)
            {
                switch (deathAnimRandomisation)
                {
                    case 1:
                        deathLegacyAnimation.Play("Death1");
                        break;
                    case 2:
                        deathLegacyAnimation.Play("Death2");
                        break;
                    case 3:
                        deathLegacyAnimation.Play("Death3");
                        break;
                    case 4:
                        deathLegacyAnimation.Play("Death4");
                        break;
                    case 5:
                        deathLegacyAnimation.Play("Death5");
                        break;
                }
            }

            if (soundSource == null) return;
            if (numberOfDeathSounds <= 0) return;
            hurtSoundRandomisation = Random.Range(1, numberOfDeathSounds + 1);

            switch (hurtSoundRandomisation)
            {
                case 1:
                    soundSource.clip = deathSound1;
                    soundSource.Play();
                    break;
                case 2:
                    soundSource.clip = deathSound2;
                    soundSource.Play();
                    break;
                case 3:
                    soundSource.clip = deathSound3;
                    soundSource.Play();
                    break;
                case 4:
                    soundSource.clip = deathSound4;
                    soundSource.Play();
                    break;
                case 5:
                    soundSource.clip = deathSound5;
                    soundSource.Play();
                    break;
            }
        }
    }

    public void UpdateState()
    {
        Animator animator = GetComponent<Animator>();
        if (!wasDeadLastFrame && IsDead())
        {
            if (gameObject.CompareTag("Player")) return;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            Destroy(gameObject, destroyDelay);
        }

        if (wasDeadLastFrame && !IsDead())
        {
            animator.Rebind();
        }

        wasDeadLastFrame = IsDead();
    }

    public bool IsDead()
    {
        return healthPoints.value <= 0;
    }

    private void AwardExperience(GameObject insigator)
    {
        Experience experience = insigator.GetComponent<Experience>();
        if (experience == null) return;

        experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return hurtAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    public void Heal(float healthToRestore)
    {
        healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
    }

    /*private void OnParticleCollision(GameObject other)
    {
        
        //if (CompareTag("Enemy"))
        if (CompareTag("Enemy"))
        {
            ApplyDamage(gameObject, 5);
            Debug.Log(other);
        }
    }*/
}