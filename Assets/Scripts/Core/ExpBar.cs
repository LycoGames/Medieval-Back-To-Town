using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxEXP(float exp)
    {
        slider.maxValue = exp;
        slider.value = 0f;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetEXP(float exp)
    {
        slider.value = exp; //tekrar exp kaldıgı yerden devam ettigi için bir 100 e 200 oluyor 100/200 gibi....
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void ResetSlider(float previousRequiredEXP)
    {
        slider.value = 0;
        slider.maxValue = GetComponent<BaseStats>().GetStat(Stat.ExperienceToLevelUp) - previousRequiredEXP; //max value must be current required EXP Points - Previous Required EXP Points
    }

}
