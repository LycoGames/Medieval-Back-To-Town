using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints = 0;
    ExpBar expBar;
    ManaBar manaBar;
    private float tempEXP = 0f;
    public event Action OnExperienceGained;

    void Start()
    {
        expBar = GameObject.FindGameObjectWithTag("Player").GetComponent<ExpBar>();
        manaBar = GameObject.FindGameObjectWithTag("Player").GetComponent<ManaBar>();
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
        if (experiencePoints >= GetMaxExp())
        {
            expBar.SetMaxEXP(GetComponent<BaseStats>().GetBaseStat(Stat.ExperienceToLevelUp)); tempEXP = 0f;
            print("exp: " + GetComponent<BaseStats>().GetBaseStat(Stat.ExperienceToLevelUp));

        } //if char levelled up.
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