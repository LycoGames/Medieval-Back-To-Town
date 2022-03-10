using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints = 0;

    public event Action OnExperienceGained;

    public void GainExperience(float experience)
    {
        experiencePoints += experience;
        OnExperienceGained();
    }

    public float GetPoints()
    {
        return experiencePoints;
    }
}