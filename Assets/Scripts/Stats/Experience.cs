using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints = 0;
    ExpBar expBar;
    private float tempEXP = 0f;
    public event Action OnExperienceGained;

    void Start()
    {
        expBar = GameObject.FindGameObjectWithTag("Player").GetComponent<ExpBar>();
        if (expBar)
        {
            expBar.SetMaxEXP(GetComponent<BaseStats>().GetBaseStat(Stat.ExperienceToLevelUp));
        }
    }

    public void GainExperience(float experience)
    {
        experiencePoints += experience;
        tempEXP += experience;
        expBar.SetEXP(tempEXP);
        if (experiencePoints >= GetMaxExp()) { expBar.SetMaxEXP(GetComponent<BaseStats>().GetBaseStat(Stat.ExperienceToLevelUp)); tempEXP = 0f; } //if char levelled up.
        OnExperienceGained();
    }

    public float GetPoints()
    {
        return experiencePoints;
    }
    public float GetMaxExp()
    {
        return GetComponent<BaseStats>().GetBaseStat(Stat.ExperienceToLevelUp);
    }


}