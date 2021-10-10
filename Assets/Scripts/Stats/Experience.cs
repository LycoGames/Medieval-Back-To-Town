using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints = 0;

    public event Action onExperienceGained;

    public void GainExperience(float experience)
    {
        experiencePoints += experience;
        onExperienceGained();
    }

    public float GetPoints()
    {
        return experiencePoints;
    }

    
}
