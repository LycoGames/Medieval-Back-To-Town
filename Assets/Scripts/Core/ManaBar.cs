using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxMana(float mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetMana(float mana)
    {
        slider.value = mana;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
