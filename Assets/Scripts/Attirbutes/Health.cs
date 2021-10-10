using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float regenerationPercentage = 50;
    [SerializeField] UnityEvent onDie;

    LazyValue<float> healthPoints;

    //bool isDead = false;

    private void Awake()
    {
        healthPoints = new LazyValue<float>(GetInitialHealth);
    }

    private float GetInitialHealth()
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
}
